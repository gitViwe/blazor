using Shared.Contract.Identity;
using System.Net.Http.Json;

namespace Blazor.Infrastructure.Manager;

internal class AuthenticationTokenManager
{
    private readonly HttpClient _httpClient;
    private readonly HubAuthenticationStateProvider _authenticationStateProvider;
    private readonly IStorageService _storageService;

    public AuthenticationTokenManager(
        IStorageService storageService,
        HubAuthenticationStateProvider authenticationStateProvider,
        HttpClient httpClient)
    {
        _storageService = storageService;
        _authenticationStateProvider = authenticationStateProvider;
        _httpClient = httpClient;
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync(Shared.Route.AuthenticationAPI.AccountEndpoint.Logout, null);

        // remove stored tokens
        await _storageService.RemoveAsync(StorageKey.Local.AuthToken);
        await _storageService.RemoveAsync(StorageKey.Local.AuthRefreshToken);

        // update the authentication state
        await _authenticationStateProvider.StateChangedAsync();
    }

    public async Task<IResponse> RefreshTokenAsync()
    {
        string token = await _storageService.GetAsync<string>(StorageKey.Local.AuthToken);
        string refreshToken = await _storageService.GetAsync<string>(StorageKey.Local.AuthRefreshToken);

        var request = new TokenRequest()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        var result = await _httpClient.PostAsJsonAsync(Shared.Route.AuthenticationAPI.AccountEndpoint.RefreshToken, request);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>();
            await _storageService.SetAsync(StorageKey.Local.AuthToken, response.Token);
            await _storageService.SetAsync(StorageKey.Local.AuthRefreshToken, response.RefreshToken);

            // update the authentication state
            await _authenticationStateProvider.StateChangedAsync();

            return Response.Success("Token refreshed.");
        }

        return Response.Fail("Token refresh failed.");
    }
}
