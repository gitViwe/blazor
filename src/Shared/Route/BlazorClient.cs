namespace Shared.Route;

/// <summary>
/// Provides the URLs for the Blazor WASM client
/// </summary>
public static class BlazorClient
{
    /// <summary>
    /// The endpoint routes for client pages
    /// </summary>
    public static class Pages
    {
        /// <summary>
        /// Endpoint routes for default example pages
        /// </summary>
        public static class DefaultExamples
        {
            public const string Home = "/";
            public const string Counter = "/counter";
            public const string FetchData = "/fetchdata";
        }

        /// <summary>
        /// Endpoint routes for authentication pages
        /// </summary>
        public static class Authentication
        {
            public const string Register = "/authentication/register";
            public const string Login = "/authentication/login";
            public const string Account = "/authentication/account";
        }
    }
}
