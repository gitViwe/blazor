import opentelemetry, { Attributes, Exception, SpanContext, SpanKind, SpanStatus, Span, SpanOptions, Context } from "@opentelemetry/api";

declare global {
  // extend Window to add a custom property
  interface Window { OpenTelemetry: OpenTelemetry; }

  interface OpenTelemetry {
    StartSpanEvent(request: StartSpanEventRequest): ContextResponse,
    StartSpanException(request: StartSpanExceptionRequest): ContextResponse,
  }

  interface StartSpanEventRequest {
    spanName: string,
    spanKind: SpanKind,
    spanEventName: string,
    spanStatus: SpanStatus,
    spanAttributes?: Attributes,
    spanEventAttributes?: Attributes,
    parentSpanContext?: SpanContext,
    traceContext?: TraceContext
  }
  
  interface StartSpanExceptionRequest {
    spanName: string,
    spanKind: SpanKind,
    spanException: Exception,
    spanStatus: SpanStatus,
    spanAttributes?: Attributes,
    parentSpanContext?: SpanContext,
    traceContext?: TraceContext
  }

  interface ContextResponse {
    spanContext?: SpanContext,
    traceContext?: TraceContext
  }

  interface TraceContext {
    traceparent: string,
    tracestate: string
  }
}

const tracer = opentelemetry.trace.getTracer(
  'blazor-wasm-client'
);

const startSpanEvent = (request: StartSpanEventRequest): ContextResponse => {
  // output values
  let spanContext: SpanContext;
  let traceContext: TraceContext;

  // resolve span options
  let spanOption: SpanOptions = request.parentSpanContext
  ? { kind: request.spanKind, attributes: request.spanAttributes, links: [ { context: request.parentSpanContext }]}
  : { kind: request.spanKind, attributes: request.spanAttributes}

  // create span
  let span: Span = tracer.startSpan(request.spanName, spanOption);

  if (request.traceContext) { traceContext = request.traceContext; }
  else {
    // mark the current span as the active span in the context
    let ctx: Context = opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
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
}

const startSpanException = (request: StartSpanExceptionRequest): ContextResponse => {
  // output values
  let spanContext: SpanContext;
  let traceContext: TraceContext;

  // resolve span options
  let spanOption: SpanOptions = request.parentSpanContext
  ? { kind: request.spanKind, attributes: request.spanAttributes, links: [ { context: request.parentSpanContext }]}
  : { kind: request.spanKind, attributes: request.spanAttributes}

  // create span
  let span: Span = tracer.startSpan(request.spanName, spanOption);

  if (request.traceContext) { traceContext = request.traceContext; }
  else {
    // mark the current span as the active span in the context
    let ctx: Context = opentelemetry.trace.setSpan(opentelemetry.context.active(), span);
    traceContext = getTraceContext(ctx);
  }

  // record span exception
  span.recordException(request.spanException)

  // set span status
  span.setStatus(request.spanStatus);

  // set output value
  spanContext = span.spanContext();

  // ensure span is ended!
  span.end();

  return { spanContext: spanContext, traceContext: traceContext };
}

const getTraceContext = (context: Context): TraceContext => {
  let output: TraceContext;

  // Serialize the traceparent and tracestate from context into an output object.
  opentelemetry.propagation.inject(context, output);

  if (output == undefined) {
    let span = opentelemetry.trace.getSpan(context);
    let spanContext = span.spanContext();
    output = { traceparent: `00-${spanContext.traceId}-${spanContext.spanId}-${String(spanContext.traceFlags).padStart(2, '0')}`, tracestate: '' };
  }
  
  // You can then pass the traceparent and tracestate
  // data to whatever mechanism you use to propagate
  // across services.
  return output;
}

// assign functions
window.OpenTelemetry = {
  StartSpanEvent: startSpanEvent,
  StartSpanException: startSpanException
};
