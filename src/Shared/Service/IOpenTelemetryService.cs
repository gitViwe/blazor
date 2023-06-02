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
    /// <returns>A <see cref="ContextResponse"/> representing the completed span</returns>
    Task<ContextResponse> StartSpanEventAsync(StartSpanEventRequest request);

    /// <summary>
    /// Creates a new span and logs an exception
    /// </summary>
    /// <param name="request">The arguments used to create the span</param>
    /// <returns>A <see cref="ContextResponse"/> representing the completed span</returns>
    Task<ContextResponse> StartSpanExceptionAsync(StartSpanExceptionRequest request);

    /// <summary>
    /// Get the span and trace context
    /// </summary>
    /// <returns>A <see cref="ContextResponse"/> representing the previously completed span</returns>
    Task<ContextResponse> GetContextResponseAsync();
}
