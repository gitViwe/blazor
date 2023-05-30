using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Shared.Contract.OpenTelemetry;

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
        var context = await _openTelemetry.StartSpanEventAsync(new()
        {
            SpanEventName = "Client navigation event",
            SpanKind = SpanKind.CLIENT,
            SpanName = $"Notified of navigation via {navigationMethod} to {e.Location}",
            SpanStatus = new(SpanStatusCode.UNSET, null),
            IsNewContext = true,
            SpanEventAttributes = new()
            {
                { nameof(e.Location), e.Location },
                { nameof(e.IsNavigationIntercepted), e.IsNavigationIntercepted },
                { nameof(e.HistoryEntryState), e.HistoryEntryState }
            },
        });
    }
}
