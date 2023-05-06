namespace Shared.Route;

/// <summary>
/// Provides the URLs for the API Endpoints
/// </summary>
public static class AuthenticationAPI
{
    public static bool IsAuthenticatedEndpoint(string endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint)) return false;
        bool isAuthenticated = AccountEndpoint.IsAuthenticatedEndpoint(endpoint);
        return isAuthenticated;
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
        public const string SuperHero = "api/superhero";
        public const string QrCode = "/api/account/qrcode";
        public const string TOTPVerify = "api/account/totp/verify";
        public const string Detail = "api/account/detail";
        public const string Picture = "api/account/picture";
        internal static bool IsAuthenticatedEndpoint(string endpoint)
        {
            string[] authenticated = new string[] { Logout, SuperHero, QrCode, TOTPVerify, Detail, Picture };
            return authenticated.Any(endpoint.Contains);
        }
    }
}
