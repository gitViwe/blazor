namespace Shared.Route;

/// <summary>
/// Provides the URLs for the API Endpoints
/// </summary>
public static class AuthenticationAPI
{
    public static bool IsAuthenticatedEndpoint(string endpoint)
    {
        bool isAnonymous = AccountEndpoint.IsAuthenticatedEndpoint(endpoint);
        return isAnonymous;
    }

    /// <summary>
    /// The endpoint routes for account endpoint
    /// </summary>
    public static class AccountEndpoint
    {
        public const string Register = "api/account/register";
        public const string Login = "api/account/login";
        public const string Logout = "api/account/logout";
        public const string RefreshToken = "api/account/refresh-token";
        internal static bool IsAuthenticatedEndpoint(string endpoint)
        {
            string[] authenticated = new string[] { Logout };
            return authenticated.Any(endpoint.Contains);
        }
    }
}
