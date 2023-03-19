namespace Shared.Contract.Identity;

public class TokenResponse
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
