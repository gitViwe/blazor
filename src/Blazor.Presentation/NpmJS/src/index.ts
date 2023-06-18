import { Resource } from "@opentelemetry/resources";
import { SemanticResourceAttributes } from "@opentelemetry/semantic-conventions";
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-base";
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { OTLPExporterNodeConfigBase } from '@opentelemetry/otlp-exporter-base';

const resource =
  Resource.default().merge(
    new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]: "BLAZOR-UI",
      [SemanticResourceAttributes.SERVICE_VERSION]: "1.0.0",
    })
  );

const provider = new WebTracerProvider({
    resource: resource,
});
const exporterOptions : OTLPExporterNodeConfigBase = {
  url: "https://api.honeycomb.io/v1/traces/",
  headers: { "x-honeycomb-team": "tsxxsanWhgi3YmHQer4zSC" },
}
const otlpExporter = new OTLPTraceExporter(exporterOptions);
const processor = new BatchSpanProcessor(otlpExporter);
provider.addSpanProcessor(processor);

provider.register();
