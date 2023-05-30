using Microsoft.JSInterop;
using Shared.Contract.OpenTelemetry;

namespace Blazor.Infrastructure.Service;

public class OpenTelemetryService : IOpenTelemetryService
{
    private readonly IJSRuntime _runtime;

    public OpenTelemetryService(IJSRuntime runtime)
    {
        _runtime = runtime;
    }

    public ValueTask<TraceContextPropagation> GetTraceContextPropagationAsync()
    {
        return _runtime.InvokeAsync<TraceContextPropagation>("OpenTelemetry.GetTraceContextPropagation");
    }

    public ValueTask<SpanContext> StartSpanEventAsync(StartSpanEventRequest request)
    {
        return _runtime.InvokeAsync<SpanContext>("OpenTelemetry.StartSpanEvent", request);
    }

    public ValueTask<SpanContext> StartSpanExceptionAsync(StartSpanExceptionRequest request)
    {
        return _runtime.InvokeAsync<SpanContext>("OpenTelemetry.StartSpanException", request);
    }
}
