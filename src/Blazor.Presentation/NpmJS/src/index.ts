import opentelemetry from "@opentelemetry/api";
import { Resource } from "@opentelemetry/resources";
import { SemanticResourceAttributes } from "@opentelemetry/semantic-conventions";
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { registerInstrumentations } from "@opentelemetry/instrumentation";
import { BatchSpanProcessor, ConsoleSpanExporter } from "@opentelemetry/sdk-trace-base";
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';

// Optionally register automatic instrumentation libraries
registerInstrumentations({
  instrumentations: [],
});

const resource =
  Resource.default().merge(
    new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]: "service-name-here",
      [SemanticResourceAttributes.SERVICE_VERSION]: "0.1.0",
    })
  );

const provider = new WebTracerProvider({
    resource: resource,
});
const exporter = new ConsoleSpanExporter();
const otlpExporter = new OTLPTraceExporter();
const processor = new BatchSpanProcessor(otlpExporter);
provider.addSpanProcessor(processor);

provider.register();

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