import opentelemetry, { propagation } from "@opentelemetry/api";
var tracer = opentelemetry.trace.getTracer('blazor-wasm-client');
var startSpanEvent = function (request) {
    // output value
    var ctx;
    // resolve span options
    var spanOption = request.parentSpanContext
        ? { kind: request.spanKind, attributes: request.spanAttributes, links: [{ context: request.parentSpanContext }] }
        : { kind: request.spanKind, attributes: request.spanAttributes };
    // resolve span context
    var span = request.isNewContext
        ? tracer.startSpan(request.spanName, spanOption)
        : tracer.startSpan(request.spanName, spanOption, opentelemetry.context.active());
    if (request.isNewContext) {
        // mark the current span as the active span in the context
        opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
    }
    // start span event
    span.addEvent(request.spanEventName, request.spanEventAttributes);
    // set span status
    span.setStatus(request.spanStatus);
    // set output value
    ctx = span.spanContext();
    // ensure span is ended!
    span.end();
    return ctx;
};
var startSpanException = function (request) {
    // output value
    var ctx;
    // resolve span options
    var spanOption = request.parentSpanContext
        ? { kind: request.spanKind, attributes: request.spanAttributes, links: [{ context: request.parentSpanContext }] }
        : { kind: request.spanKind, attributes: request.spanAttributes };
    // resolve span context
    var span = request.isNewContext
        ? tracer.startSpan(request.spanName, spanOption)
        : tracer.startSpan(request.spanName, spanOption, opentelemetry.context.active());
    if (request.isNewContext) {
        // mark the current span as the active span in the context
        opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
    }
    // record span exception
    span.recordException(request.spanException);
    // set span status
    span.setStatus(request.spanStatus);
    // set output value
    ctx = span.spanContext();
    // ensure span is ended!
    span.end();
    // return output
    return ctx;
};
var getTraceContextPropagation = function () {
    var output;
    // Serialize the traceparent and tracestate from context into an output object.
    propagation.inject(opentelemetry.context.active(), output);
    // You can then pass the traceparent and tracestate
    // data to whatever mechanism you use to propagate
    // across services.
    return output;
};
// assign functions
window.OpenTelemetry = {
    StartSpanEvent: startSpanEvent,
    StartSpanException: startSpanException,
    GetTraceContextPropagation: getTraceContextPropagation
};
//# sourceMappingURL=open_telemetry.js.map