import opentelemetry from "@opentelemetry/api";
//...

const tracer = opentelemetry.trace.getTracer(
  'my-service-tracer'
);

// Create a span. A span must be closed.
tracer.startActiveSpan('main', (span) => {
    for (let i = 0; i < 10; i += 1) {
      console.log(i);
    }
  
    // Be sure to end the span!
    span.end();
});

tracer.startActiveSpan('app.new-span', (span) => {
    // do some work...
  
    // Add an attribute to the span
    span.setAttribute('attribute1', 'value1');
  
    span.end();
});

tracer.startActiveSpan(
    'app.new-span',
    { attributes: { attribute1: 'value1' } },
    (span) => {
      // do some work...
  
      span.end();
    }
  );
  

const activeSpan = opentelemetry.trace.getActiveSpan();

// do something with the active span, optionally ending it if that is appropriate for your use case.