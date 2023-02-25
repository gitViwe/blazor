﻿using Shared.Contract.Identity;
using System.Net.Http.Json;
using WebAssembly.Infrastructure.Authentication;

namespace WebAssembly.Infrastructure.Manager;

internal class HubUserManager : IHubUserManager
{
    private readonly HttpClient _httpClient;
    private readonly HubAuthenticationStateProvider _authenticationStateProvider;
    private readonly IStorageService _storageService;

    public HubUserManager(
        HttpClient httpClient,
        HubAuthenticationStateProvider authenticationStateProvider,
        IStorageService storageService)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _storageService = storageService;
    }

    public async Task<ClaimsPrincipal> CurrentUserAsync()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User;
    }

    public async Task<IResponse> LoginAsync(ILoginRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(Shared.Route.AuthenticationAPI.AcccountEndpoint.Login, request);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>();
            await _storageService.SetAsync(StorageKey.Local.AuthToken, response.Token);
            await _storageService.SetAsync(StorageKey.Local.AuthRefreshToken, response.RefreshToken);

            // update the authentication state
            await _authenticationStateProvider.StateChangedAsync();

            return Response.Success("Login successful.");
        }

        return Response.Fail("Login failed.");
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync(Shared.Route.AuthenticationAPI.AcccountEndpoint.Logout, null);

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

        var result = await _httpClient.PostAsJsonAsync(Shared.Route.AuthenticationAPI.AcccountEndpoint.RefreshToken, request);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>();
            await _storageService.SetAsync(StorageKey.Local.AuthToken, response.Token);
            await _storageService.SetAsync(StorageKey.Local.AuthRefreshToken, response.RefreshToken);

            // update the authentication state
            await _authenticationStateProvider.StateChangedAsync();

            return Response.Success();
        }

        return Response.Fail();
    }

    public async Task<IResponse> RegisterAsync(IRegisterRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(Shared.Route.AuthenticationAPI.AcccountEndpoint.Register, request);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.ToResponseAsync<TokenResponse>();
            await _storageService.SetAsync(StorageKey.Local.AuthToken, response.Token);
            await _storageService.SetAsync(StorageKey.Local.AuthRefreshToken, response.RefreshToken);

            // update the authentication state
            await _authenticationStateProvider.StateChangedAsync();

            return Response.Success("Registration successful.");
        }

        return Response.Fail("Registration failed.");
    }
}
