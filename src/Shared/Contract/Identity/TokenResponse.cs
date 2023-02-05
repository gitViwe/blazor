using gitViwe.Shared;

namespace Shared.Contract.Identity;

public class TokenResponse : ITokenResponse
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
