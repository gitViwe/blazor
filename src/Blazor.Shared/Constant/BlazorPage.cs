namespace Blazor.Shared.Constant;

public class BlazorPage
{
    public static class DefaultExamples
    {
        public const string Counter = "counter";
        public const string FetchData = "fetchdata";
    }
    
    public static class Documentation
    {
        public const string Swagger = "documentation/swagger";
        public const string Scalar = "documentation/scalar";
        public const string GraphQl = "documentation/graphql";
    }
    
    public static class Observability
    {
        public const string OpenTelemetry = "observability/open-telemetry";
        public const string HealthCheck = "observability/health-check";
    }
    
    public static class Security
    {
        public const string WebAuthn = "security/webauthn";
        public const string Mfa = "security/mfa";
    }
    
    public static class Communication
    {
        public const string SignalR = "paradigm/signalr";
        public const string Grpc = "paradigm/grpc";
    }
}