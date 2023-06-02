import opentelemetry from "@opentelemetry/api";
var tracer = opentelemetry.trace.getTracer('blazor-wasm-client');
var startSpanEvent = function (request) {
    // output values
    var spanContext;
    var traceContext;
    // resolve span options
    var spanOption = request.parentSpanContext
        ? { kind: request.spanKind, attributes: request.spanAttributes, links: [{ context: request.parentSpanContext }] }
        : { kind: request.spanKind, attributes: request.spanAttributes };
    // create span
    var span = tracer.startSpan(request.spanName, spanOption);
    if (request.traceContext) {
        traceContext = request.traceContext;
    }
    else {
        // mark the current span as the active span in the context
        var ctx = opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
        traceContext = getTraceContext(ctx);
    }
    // start span event
    span.addEvent(request.spanEventName, request.spanEventAttributes);
    // set span status
    span.setStatus(request.spanStatus);
    // set output value
    spanContext = span.spanContext();
    // ensure span is ended!
    span.end();
    return { spanContext: spanContext, traceContext: traceContext };
};
var startSpanException = function (request) {
    // output values
    var spanContext;
    var traceContext;
    // resolve span options
    var spanOption = request.parentSpanContext
        ? { kind: request.spanKind, attributes: request.spanAttributes, links: [{ context: request.parentSpanContext }] }
        : { kind: request.spanKind, attributes: request.spanAttributes };
    // create span
    var span = tracer.startSpan(request.spanName, spanOption);
    if (request.traceContext) {
        traceContext = request.traceContext;
    }
    else {
        // mark the current span as the active span in the context
        var ctx = opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
        traceContext = getTraceContext(ctx);
    }
    // record span exception
    span.recordException(request.spanException);
    // set span status
    span.setStatus(request.spanStatus);
    // set output value
    spanContext = span.spanContext();
    // ensure span is ended!
    span.end();
    return { spanContext: spanContext, traceContext: traceContext };
};
var getTraceContext = function (context) {
    var output;
    // Serialize the traceparent and tracestate from context into an output object.
    opentelemetry.propagation.inject(context, output);
    if (output == undefined) {
        var span = opentelemetry.trace.getSpan(context);
        var spanContext = span.spanContext();
        output = { traceparent: "00-".concat(spanContext.traceId, "-").concat(spanContext.spanId, "-").concat(String(spanContext.traceFlags).padStart(2, '0')), tracestate: '' };
    }
    // You can then pass the traceparent and tracestate
    // data to whatever mechanism you use to propagate
    // across services.
    return output;
};
// assign functions
window.OpenTelemetry = {
    StartSpanEvent: startSpanEvent,
    StartSpanException: startSpanException
};
//# sourceMappingURL=open_telemetry.js.map