using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Shared.Route;

namespace Blazor.Infrastructure.Service;

internal class LocationChangedInterceptorService : ILocationChangedInterceptorService
{
    private readonly NavigationManager _navigationManager;
    private readonly IOpenTelemetryService _openTelemetry;

    public LocationChangedInterceptorService(NavigationManager navigationManager, IOpenTelemetryService openTelemetry)
    {
        _navigationManager = navigationManager;
        _openTelemetry = openTelemetry;
    }

    public void DisposeEvent()
    {
        _navigationManager.LocationChanged -= LocationChangedAsync;
    }

    public void RegisterEvent()
    {
        _navigationManager.LocationChanged += LocationChangedAsync;
    }

    private async void LocationChangedAsync(object? sender, LocationChangedEventArgs e)
    {
        string navigationMethod = e.IsNavigationIntercepted ? "HTML" : "code";
        await _openTelemetry.StartSpanEventAsync(
            spanName: "Page Navigation",
            eventName: $"Blazor UI navigation via {navigationMethod} to the [{GetPageRouteName(e.Location)}] page.",
            eventTags: new()
            {
                { nameof(e.Location), e.Location },
                { nameof(e.IsNavigationIntercepted), e.IsNavigationIntercepted },
                { nameof(e.HistoryEntryState), e.HistoryEntryState }
            });
    }

    private static string GetPageRouteName(string location)
    {
        var uri = new Uri(location);
        return PageRouteMapper.TryGetValue(uri.PathAndQuery, out var route)
            ? route : "undefined";
    }

    private static readonly Dictionary<string, string> PageRouteMapper = new()
    {
        { BlazorClient.Pages.Authentication.Account, "Account"},
        { BlazorClient.Pages.Authentication.Login, "Login"},
        { BlazorClient.Pages.Authentication.Register, "Register"},

        { BlazorClient.Pages.DefaultExamples.Counter, "Counter"},
        { BlazorClient.Pages.DefaultExamples.FetchData, "Fetch Data"},
        { BlazorClient.Pages.DefaultExamples.Home, "Home"},

        { BlazorClient.Pages.Documentation.GraphQL, "GraphQL"},
        { BlazorClient.Pages.Documentation.Swagger, "Swagger"},
    };
}
