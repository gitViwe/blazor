namespace Blazor.Presentation.Constant;

/// <summary>
/// Provides the static file location paths
/// </summary>
public static class Asset
{
    /// <summary>
    /// Image static file location paths
    /// </summary>
    public static class Image
    {
        private const string _rootFolder = "_content/Blazor.Presentation/";
        public const string GitHub = _rootFolder +"image/astro-mona.webp";
        public const string PortfolioShowcase = _rootFolder +"image/ydps-2015.png";
        public const string TwoFactorAuthentication = _rootFolder + "image/two-factor-authentication.png";
    }
}
