using System.Text.Json.Serialization;

namespace Shared.Contract.OpenTelemetry;

public class StartSpanEventRequest
{
    [JsonPropertyName("spanName")]
    public required string SpanName { get; set; }

    [JsonPropertyName("spanEventName")]
    public required string SpanEventName { get; set; }

    [JsonPropertyName("spanStatus")]
    public SpanStatus SpanStatus { get; set; } = new(SpanStatusCode.UNSET, null);

    [JsonPropertyName("spanKind")]
    public SpanKind SpanKind { get; set; } = SpanKind.INTERNAL;

    [JsonPropertyName("spanAttributes")]
    public Dictionary<string, object?> SpanAttributes { get; set; } = new();

    [JsonPropertyName("spanEventAttributes")]
    public Dictionary<string, object?> SpanEventAttributes { get; set; } = new();

    [JsonPropertyName("parentSpanContext")]
    public SpanContext? ParentSpanContext { get; set; }

    [JsonPropertyName("traceContext")]
    public TraceContext? TraceContext { get; set; }
}

public class StartSpanExceptionRequest
{
    [JsonPropertyName("spanName")]
    public required string SpanName { get; set; }

    [JsonPropertyName("spanException")]
    public required SpanException SpanException { get; set; }
    
    [JsonPropertyName("spanKind")]
    public SpanKind SpanKind { get; set; } = SpanKind.INTERNAL;

    [JsonPropertyName("spanStatus")]
    public SpanStatus SpanStatus { get; set; } = new(SpanStatusCode.UNSET, null);

    [JsonPropertyName("spanAttributes")]
    public Dictionary<string, object?> SpanAttributes { get; set; } = new();

    [JsonPropertyName("parentSpanContext")]
    public SpanContext? ParentSpanContext { get; set; }

    [JsonPropertyName("traceContext")]
    public TraceContext? TraceContext { get; set; }
}

public class ContextResponse
{
    [JsonPropertyName("spanContext")]
    public SpanContext? SpanContext { get; set; }

    [JsonPropertyName("traceContext")]
    public TraceContext? TraceContext { get; set; }
}

public class TraceContext
{
    [JsonPropertyName("traceparent")]
    public string Traceparent { get; set; } = string.Empty;

    [JsonPropertyName("tracestate")]
    public string Tracestate { get; set; } = string.Empty;
}

public record SpanContext
{
    [JsonPropertyName("traceId")]
    public string TraceId { get; set; } = string.Empty;

    [JsonPropertyName("spanId")]
    public string SpanId { get; set; } = string.Empty;

    [JsonPropertyName("isRemote")]
    public bool? IsRemote { get; set; }

    [JsonPropertyName("traceFlags")]
    public int TraceFlags { get; set; }

    [JsonPropertyName("traceState")]
    public object? TraceState { get; set; }
}

public record SpanStatus(
    [property: JsonPropertyName("code")] SpanStatusCode Code,
    [property: JsonPropertyName("message")] string? Message);

public record SpanException(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("stack")] string? StackTrace);

public enum SpanKind
{
    /// <summary>
    /// Default value. Indicates that the span is used internally.
    /// </summary>
    INTERNAL = 0,

    /// <summary>
    /// Indicates that the span covers server-side handling of an RPC or other remote request.
    /// </summary>
    SERVER = 1,

    /// <summary>
    /// Indicates that the span covers the client-side wrapper around an RPC or other remote request.
    /// </summary>
    CLIENT = 2,

    /// <summary>
    /// Indicates that the span describes producer sending a message to a
    /// broker. Unlike client and server, there is no direct critical path latency
    /// relationship between producer and consumer spans.
    /// </summary>
    PRODUCER = 3,

    /// <summary>
    /// Indicates that the span describes consumer receiving a message from a
    /// broker. Unlike client and server, there is no direct critical path latency
    /// relationship between producer and consumer spans.
    /// </summary>
    CONSUMER = 4
}

public enum SpanStatusCode
{
    /// <summary>
    /// The default status.
    /// </summary>
    UNSET = 0,

    /// <summary>
    /// The operation has been validated by an Application developer or Operator to have completed successfully.
    /// </summary>
    OK = 1,

    /// <summary>
    /// The operation contains an error.
    /// </summary>
    ERROR = 2
}
