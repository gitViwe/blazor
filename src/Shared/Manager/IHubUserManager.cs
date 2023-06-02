using Shared.Contract;
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

    /// <summary>
    /// Gets a QR code image to set up TOTP and stores it in local storage using the key <seealso cref="Constant.StorageKey.Identity.QrCodeImage"/>
    /// </summary>
    /// <returns>The response message</returns>
    Task<IBlazorResponse> GetQrCodeAsync();

    /// <summary>
    /// Verifies the TOTP from the QrCode set up
    /// </summary>
    /// <param name="request">The token to verify with</param>
    /// <returns>The response message</returns>
    Task<IResponse> VerifyTOTPAsync(TOTPVerifyRequest request);

    /// <summary>
    /// Update the user's details on the API
    /// </summary>
    /// <param name="request">The user details to update</param>
    /// <returns>The response message</returns>
    Task<IResponse> UpdateDetailsAsync(UpdateUserRequest request);

    /// <summary>
    /// Upload a profile image to the API
    /// </summary>
    /// <param name="content">The image file to upload</param>
    /// <param name="fileName">The image file name</param>
    /// <returns>The response message</returns>
    Task<IResponse> UploadImageAsync(Stream content, string fileName);

    /// <summary>
    /// Get the current user's details from the API
    /// </summary>
    /// <returns>An instance of <see cref="UserDetailResponse"/> with current user's details</returns>
    Task<IResponse<UserDetailResponse>> GetUserDetailAsync();
}
