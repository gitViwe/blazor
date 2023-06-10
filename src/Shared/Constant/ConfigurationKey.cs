namespace Shared.Constant;

/// <summary>
/// Provides the keys to get the values from AppSettings
/// </summary>
public static class ConfigurationKey
{
    /// <summary>
    /// Provides the keys to get the values from the APIConfiguration section
    /// </summary>
    public static class API
    {
        public const string Secret = "APIConfiguration:Secret";
        public const string ClientUrl = "APIConfiguration:ClientUrl";
        public const string ServerUrl = "APIConfiguration:ServerUrl";
        public const string GraphUrl = "APIConfiguration:GraphUrl";
    }
}
