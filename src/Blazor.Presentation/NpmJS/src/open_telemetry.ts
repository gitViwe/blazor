import opentelemetry, { Attributes, Exception, SpanContext, SpanKind, SpanStatus, Span, propagation, SpanOptions } from "@opentelemetry/api";

declare global {
  // extend Window to add a custom property
  interface Window { OpenTelemetry: OpenTelemetry; }

  interface OpenTelemetry {
    StartSpanEvent(request: StartSpanEventRequest): SpanContext,
    StartSpanException(request: StartSpanExceptionRequest): SpanContext,
    GetTraceContextPropagation(): TraceContextPropagation
  }

  interface StartSpanEventRequest {
    isNewContext: boolean,
    spanName: string,
    spanKind: SpanKind,
    spanEventName: string,
    spanStatus: SpanStatus,
    spanAttributes?: Attributes,
    spanEventAttributes?: Attributes,
    parentSpanContext?: SpanContext
  }
  
  interface StartSpanExceptionRequest {
    isNewContext: boolean,
    spanName: string,
    spanKind: SpanKind,
    spanException: Exception,
    spanStatus: SpanStatus,
    spanAttributes?: Attributes,
    parentSpanContext?: SpanContext
  }

  interface TraceContextPropagation {
    traceparent: string,
    tracestate: string
  }
}

const tracer = opentelemetry.trace.getTracer(
  'blazor-wasm-client'
);

const startSpanEvent = (request: StartSpanEventRequest): SpanContext => {
  // output value
  let ctx: SpanContext;

  // resolve span options
  let spanOption: SpanOptions = request.parentSpanContext
  ? { kind: request.spanKind, attributes: request.spanAttributes, links: [ { context: request.parentSpanContext }]}
  : { kind: request.spanKind, attributes: request.spanAttributes}

  // resolve span context
  let span: Span = request.isNewContext
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
}

const startSpanException = (request: StartSpanExceptionRequest): SpanContext => {
  // output value
  let ctx: SpanContext;

  // resolve span options
  let spanOption: SpanOptions = request.parentSpanContext
  ? { kind: request.spanKind, attributes: request.spanAttributes, links: [ { context: request.parentSpanContext }]}
  : { kind: request.spanKind, attributes: request.spanAttributes}

  // resolve span context
  let span: Span = request.isNewContext
  ? tracer.startSpan(request.spanName, spanOption)
  : tracer.startSpan(request.spanName, spanOption, opentelemetry.context.active());

  if (request.isNewContext) {
    // mark the current span as the active span in the context
    opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
  }

  // record span exception
  span.recordException(request.spanException)

  // set span status
  span.setStatus(request.spanStatus);

  // set output value
  ctx = span.spanContext();

  // ensure span is ended!
  span.end();

  // return output
  return ctx;
}

const getTraceContextPropagation = (): TraceContextPropagation => {
  let output: TraceContextPropagation;

  // Serialize the traceparent and tracestate from context into an output object.
  propagation.inject(opentelemetry.context.active(), output);
  
  // You can then pass the traceparent and tracestate
  // data to whatever mechanism you use to propagate
  // across services.
  return output;
}

// assign functions
window.OpenTelemetry = {
  StartSpanEvent: startSpanEvent,
  StartSpanException: startSpanException,
  GetTraceContextPropagation: getTraceContextPropagation
};
