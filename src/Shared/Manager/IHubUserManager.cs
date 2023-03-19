using Shared.Contract.Identity;
using System.Security.Claims;

namespace Shared.Manager;

public interface IHubUserManager
{
    /// <summary>
    /// Get the current user
    /// </summary>
    /// <returns>The claims principal representing the current user</returns>
    Task<ClaimsPrincipal> CurrentUserAsync();

    /// <summary>
    /// Send a login request to the API
    /// </summary>
    /// <param name="request">The user details required for login</param>
    /// <returns>The response message</returns>
    Task<IResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Clears all credentials on the client
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Attempts to get a new JWT token
    /// </summary>
    /// <returns>A response message on failure</returns>
    Task<IResponse> RefreshTokenAsync();

    /// <summary>
    /// Send a registration request to the API
    /// </summary>
    /// <param name="request">The user details required for registration</param>
    /// <returns>The response message</returns>
    Task<IResponse> RegisterAsync(RegisterRequest request);
}
