import { Resource } from "@opentelemetry/resources";
import { SemanticResourceAttributes } from "@opentelemetry/semantic-conventions";
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-base";
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';

const resource =
  Resource.default().merge(
    new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]: "BLAZOR-UI-LOCAL",
      [SemanticResourceAttributes.SERVICE_VERSION]: "1.0.0",
    })
  );

const provider = new WebTracerProvider({
    resource: resource,
});
const otlpExporter = new OTLPTraceExporter();
const processor = new BatchSpanProcessor(otlpExporter);
provider.addSpanProcessor(processor);

provider.register();
