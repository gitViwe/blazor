namespace Shared.Constant;

/// <summary>
/// Provides the keys to the client storage
/// </summary>
public class StorageKey
{
    /// <summary>
    /// Provides the keys to get the values from the local storage
    /// </summary>
    public static class Local
    {
        public const string AuthToken = "AuthToken";
        public const string AuthRefreshToken = "AuthRefreshToken";
        public const string AvatarImage = "AvatarImage";
        public const string QrCodeImage = "QrCodeImage";
    }
}
