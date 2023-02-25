namespace Shared.Route;

/// <summary>
/// Provides the URLs for the API Endpoints
/// </summary>
public static class AuthenticationAPI
{
    /// <summary>
    /// The endpoint routes for account endpoint
    /// </summary>
    public static class AcccountEndpoint
    {
        public const string Register = "api/account/register";
        public const string Login = "api/account/login";
        public const string Logout = "api/account/logout";
        public const string RefreshToken = "api/account/refresh-token";
        public static bool IsAnonymousEndpoint(string endpoint)
        {
            string[] anonymous = new string[] { Register, Login, RefreshToken };
            return anonymous.Any(endpoint.Contains);
        }
    }
}
