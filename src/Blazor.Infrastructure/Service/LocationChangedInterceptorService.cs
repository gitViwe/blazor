using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Shared.Contract.OpenTelemetry;
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
        var context = await _openTelemetry.GetContextResponseAsync();
        await _openTelemetry.StartSpanEventAsync(new()
        {
            SpanKind = SpanKind.CLIENT,
            SpanName = GetPageRouteName(e.Location),
            SpanStatus = new(SpanStatusCode.UNSET, null),
            SpanEventName = $"Blazor UI navigation via {navigationMethod} to [{GetPageRouteName(e.Location)}]",
            SpanEventAttributes = new()
            {
                { nameof(e.Location), e.Location },
                { nameof(e.IsNavigationIntercepted), e.IsNavigationIntercepted },
                { nameof(e.HistoryEntryState), e.HistoryEntryState }
            },
            ParentSpanContext = context.SpanContext,
        });
    }

    private static string GetPageRouteName(string location)
    {
        var uri = new Uri(location);
        return PageRouteMapper.TryGetValue(uri.PathAndQuery, out var route)
            ? route : "undefined";
    }

    private static Dictionary<string, string> PageRouteMapper = new()
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
