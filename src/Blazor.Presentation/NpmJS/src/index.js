var _a;
import { Resource } from "@opentelemetry/resources";
import { SemanticResourceAttributes } from "@opentelemetry/semantic-conventions";
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-base";
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
var resource = Resource.default().merge(new Resource((_a = {},
    _a[SemanticResourceAttributes.SERVICE_NAME] = "BLAZOR-UI",
    _a[SemanticResourceAttributes.SERVICE_VERSION] = "1.0.0",
    _a)));
var provider = new WebTracerProvider({
    resource: resource,
});
var otlpExporter = new OTLPTraceExporter();
var processor = new BatchSpanProcessor(otlpExporter);
provider.addSpanProcessor(processor);
provider.register();
//# sourceMappingURL=index.js.map