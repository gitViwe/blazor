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

    public async Task<ContextResponse> StartSpanEventAsync(StartSpanEventRequest request)
    {
        var response = await _runtime.InvokeAsync<ContextResponse>("OpenTelemetry.StartSpanEvent", request);
        await SetContextResponseAsync(response);
        return response;
    }

    public async Task<ContextResponse> StartSpanExceptionAsync(StartSpanExceptionRequest request)
    {
        var response = await _runtime.InvokeAsync<ContextResponse>("OpenTelemetry.StartSpanException", request);
        await SetContextResponseAsync(response);
        return response;
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
