using Shared.Contract.OpenTelemetry;

namespace Shared.Service;

/// <summary>
/// Exposes Open Telemetry API methods
/// </summary>
public interface IOpenTelemetryService
{
    /// <summary>
    /// Creates a new span and logs an event
    /// </summary>
    /// <param name="request">The arguments used to create the span</param>
    /// <returns>A <see cref="SpanContext"/> representing the completed span</returns>
    ValueTask<SpanContext> StartSpanEventAsync(StartSpanEventRequest request);

    /// <summary>
    /// Creates a new span and logs an exception
    /// </summary>
    /// <param name="request">The arguments used to create the span</param>
    /// <returns>A <see cref="SpanContext"/> representing the completed span</returns>
    ValueTask<SpanContext> StartSpanExceptionAsync(StartSpanExceptionRequest request);

    /// <summary>
    /// You can then pass the trace parent and trace state data to the HTTP headers to propagate across services
    /// </summary>
    /// <returns>A <see cref="TraceContextPropagation"/> containing the trace propagating values</returns>
    ValueTask<TraceContextPropagation> GetTraceContextPropagationAsync();
}
