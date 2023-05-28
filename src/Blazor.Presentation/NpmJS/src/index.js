"use strict";
var _a;
Object.defineProperty(exports, "__esModule", { value: true });
var api_1 = require("@opentelemetry/api");
var resources_1 = require("@opentelemetry/resources");
var semantic_conventions_1 = require("@opentelemetry/semantic-conventions");
var sdk_trace_web_1 = require("@opentelemetry/sdk-trace-web");
var instrumentation_1 = require("@opentelemetry/instrumentation");
var sdk_trace_base_1 = require("@opentelemetry/sdk-trace-base");
var exporter_trace_otlp_http_1 = require("@opentelemetry/exporter-trace-otlp-http");
// Optionally register automatic instrumentation libraries
(0, instrumentation_1.registerInstrumentations)({
    instrumentations: [],
});
var resource = resources_1.Resource.default().merge(new resources_1.Resource((_a = {},
    _a[semantic_conventions_1.SemanticResourceAttributes.SERVICE_NAME] = "service-name-here",
    _a[semantic_conventions_1.SemanticResourceAttributes.SERVICE_VERSION] = "0.1.0",
    _a)));
var provider = new sdk_trace_web_1.WebTracerProvider({
    resource: resource,
});
var exporter = new sdk_trace_base_1.ConsoleSpanExporter();
var otlpExporter = new exporter_trace_otlp_http_1.OTLPTraceExporter();
var processor = new sdk_trace_base_1.BatchSpanProcessor(otlpExporter);
provider.addSpanProcessor(processor);
provider.register();
var tracer = api_1.default.trace.getTracer('my-service-tracer');
// Create a span. A span must be closed.
tracer.startActiveSpan('main', function (span) {
    for (var i = 0; i < 10; i += 1) {
        console.log(i);
    }
    // Be sure to end the span!
    span.end();
});
//# sourceMappingURL=index.js.map