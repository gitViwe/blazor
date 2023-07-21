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
    /// <param name="spanName">The span name</param>
    /// <param name="eventName">The name of the span event</param>
    /// <returns>A <see cref="Task"/> representing the completed span</returns>
    Task StartSpanEventAsync(string spanName, string eventName);

    /// <summary>
    /// Creates a new span and logs an event
    /// </summary>
    /// <param name="spanName">The span name</param>
    /// <param name="eventName">The name of the span event</param>
    /// <param name="eventTags">The additional data as key-value pairs</param>
    /// <returns>A <see cref="Task"/> representing the completed span</returns>
    Task StartSpanEventAsync(string spanName, string eventName, Dictionary<string, object?> eventTags);

    /// <summary>
    /// Creates a new span and logs an exception
    /// </summary>
    /// <param name="spanName">The span name</param>
    /// <param name="exceptionCode">The common code of the exception type</param>
    /// <param name="exception">The exception to record</param>
    /// <param name="spanStatusMessage">The span status message</param>
    /// <returns>A <see cref="Task"/> representing the completed span</returns>
    Task StartSpanExceptionAsync(string spanName, string exceptionCode, Exception exception, string? spanStatusMessage = null);

    /// <summary>
    /// Get the span and trace context
    /// </summary>
    /// <returns>A <see cref="ContextResponse"/> representing the previously completed span</returns>
    Task<ContextResponse> GetContextResponseAsync();
}
