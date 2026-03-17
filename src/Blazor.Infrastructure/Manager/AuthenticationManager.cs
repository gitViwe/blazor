using System.Net.Http.Json;
using Blazor.Shared.Contract;
using gitViwe.Shared;

namespace Blazor.Infrastructure.Manager;

internal class AuthenticationManager(
    IJSRuntime jsRuntime,
    IGatewayClient gateway,
    HubAuthenticationStateProvider stateProvider)
{
    public async Task<IResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.PostAsJsonAsync("auth/account/register", request, cancellationToken);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>(token: cancellationToken);

            if (response is null)
            {
                return Response.Fail("Registration failed.");
            }

            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthToken, response.Token, cancellationToken);
            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthRefreshToken, response.RefreshToken, cancellationToken);

            // update the authentication state
            await stateProvider.StateChangedAsync();

            return Response.Success("Registration successful.");
        }

        return Response.Fail("Registration failed.");
    }
    
    public async Task<IResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.PostAsJsonAsync("auth/account/login", request, cancellationToken);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>(token: cancellationToken);

            if (response is null)
            {
                return Response.Fail("Login failed.");
            }

            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthToken, response.Token, cancellationToken);
            await jsRuntime.SessionStorageSetAsync(HubStorageKey.Identity.AuthRefreshToken, response.RefreshToken, cancellationToken);

            // update the authentication state
            await stateProvider.StateChangedAsync();

            return Response.Success("Login successful.");
        }

        return Response.Fail("Login failed.");
    }
    
    public async Task<ITypedResponse<UserDetailResponse>> GetUserDetailAsync(CancellationToken cancellationToken)
    {
        var result = await gateway.HttpClient.GetAsync("auth/account/detail", cancellationToken);

        if (false == result.IsSuccessStatusCode)
        {
            return TypedResponse<UserDetailResponse>.Fail("Failed to retrieve user details.");
        }

        var response = await result.ToResponseAsync<UserDetailResponse>(token: cancellationToken);

        if (response is null)
        {
            return TypedResponse<UserDetailResponse>.Fail("Failed to retrieve user details.");
        }
        
        return TypedResponse<UserDetailResponse>.Success("User details retrieved.", response);
    }
}