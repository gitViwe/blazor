namespace Blazor.Component.Constant;

public static class BlazorPage
{
    public static class Authentication
    {
        public const string Register = "/authentication/register";
        public const string Login = "/authentication/login";
        public const string Account = "/authentication/account";
    }
    
    public static class DefaultExamples
    {
        public const string Home = "/";
        public const string Counter = "/counter";
        public const string FetchData = "/fetchdata";
    }
    
    public static class Documentation
    {
        public const string Swagger = "/documentation/swagger";
        public const string GraphQL = "/documentation/graphql";
    }
    
    public static class Paradigm
    {
        public const string OpenTelemetry = "/paradigm/open-telemetry";
        public const string SignalR = "/paradigm/signalr";
        public const string HealthCheck = "/paradigm/health-check";
    }
}