namespace Shared.Constant;

/// <summary>
/// Provides the keys to the client storage
/// </summary>
public class StorageKey
{
    /// <summary>
    /// Provides the keys to get the values from the local storage
    /// </summary>
    public static class Identity
    {
        public const string AuthToken = "Identity.AuthToken";
        public const string AuthRefreshToken = "Identity.AuthRefreshToken";
        public const string UserDetail = "Identity.UserDetail";
    }

    /// <summary>
    /// Provides the keys to get the values from the local storage
    /// </summary>
    public static class OpenTelemetry
    {
        public const string SpanContext = "OpenTelemetry.SpanContext";
        public const string TraceContext = "OpenTelemetry.TraceContext";
    }
}
