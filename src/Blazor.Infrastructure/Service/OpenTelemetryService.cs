using Microsoft.JSInterop;
using Shared.Contract.OpenTelemetry;
using System.Text.Json;

namespace Blazor.Infrastructure.Service;

public class OpenTelemetryService : IOpenTelemetryService
{
    private readonly IJSRuntime _runtime;
    private readonly IStorageService _storage;

    public OpenTelemetryService(IJSRuntime runtime, IStorageService storage)
    {
        _runtime = runtime;
        _storage = storage;
    }

    public async Task StartSpanEventAsync(string spanName, string eventName)
    {
        var context = await GetContextResponseAsync();

        var request = new StartSpanEventRequest()
        {
            SpanName = spanName,
            SpanEventName = eventName,
            ParentSpanContext = context.SpanContext,
        };

        var response = await _runtime.InvokeAsync<ContextResponse>("OpenTelemetry.StartSpanEvent", request);
        await SetContextResponseAsync(response);
    }

    public async Task StartSpanExceptionAsync(string spanName, string exceptionCode, Exception exception, string? spanStatusMessage = null)
    {
        var context = await GetContextResponseAsync();

        var request = new StartSpanExceptionRequest()
        {
            SpanName = spanName,
            SpanException = new SpanException(
                Code: exceptionCode,
                Message: exception.Message,
                Name: exception.Source,
                StackTrace: exception.StackTrace),
            SpanStatus = new(SpanStatusCode.ERROR, spanStatusMessage),
            ParentSpanContext = context.SpanContext,
        };

        var response = await _runtime.InvokeAsync<ContextResponse>("OpenTelemetry.StartSpanException", request);
        await SetContextResponseAsync(response);
    }

    public async Task StartSpanEventAsync(string spanName, string eventName, Dictionary<string, object?> eventTags)
    {
        var context = await GetContextResponseAsync();

        var request = new StartSpanEventRequest()
        {
            SpanName = spanName,
            SpanEventName = eventName,
            SpanEventAttributes = eventTags,
            ParentSpanContext = context.SpanContext,
        };

        var response = await _runtime.InvokeAsync<ContextResponse>("OpenTelemetry.StartSpanEvent", request);
        await SetContextResponseAsync(response);
    }

    public async Task<ContextResponse> GetContextResponseAsync()
    {
        ContextResponse response = new();

        // get the saved context
        var traceContext = await _storage.GetAsync<string>(StorageKey.OpenTelemetry.TraceContext);
        var spanContext = await _storage.GetAsync<string>(StorageKey.OpenTelemetry.SpanContext);

        if (!string.IsNullOrWhiteSpace(traceContext))
        {
            response.TraceContext = JsonSerializer.Deserialize<TraceContext>(traceContext);
        }

        if (!string.IsNullOrWhiteSpace(spanContext))
        {
            response.SpanContext = JsonSerializer.Deserialize<SpanContext>(spanContext);
        }

        return response;
    }

    private async Task SetContextResponseAsync(ContextResponse response)
    {
        if (response?.SpanContext is not null)
        {
            await _storage.SetAsync(StorageKey.OpenTelemetry.SpanContext, JsonSerializer.Serialize(response.SpanContext));
        }

        if (response?.TraceContext is not null)
        {
            await _storage.SetAsync(StorageKey.OpenTelemetry.TraceContext, JsonSerializer.Serialize(response.TraceContext));
        }
    }
}
