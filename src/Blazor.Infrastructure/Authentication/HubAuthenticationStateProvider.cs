using Microsoft.AspNetCore.Components.Authorization;
using Shared.Contract.Identity;
using System.Text.Json;

namespace Blazor.Infrastructure.Authentication;

internal class HubAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IStorageService _storageService;

    public HubAuthenticationStateProvider(IStorageService storageService)
    {
        _storageService = storageService;
    }

    /// <summary>
    /// The current user's claims principal
    /// </summary>
    public ClaimsPrincipal AuthenticationStateUser { get; private set; } = new ClaimsPrincipal(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // get the saved JWT token
        var savedToken = await _storageService.GetAsync<string>(StorageKey.Identity.AuthToken);

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            // return empty credentials if no token found
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // get the authentication state using the saved token
        var authSatate = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(await GetClaimsFromJwtAsync(savedToken), "jwt")));

        // get the authentication state user value
        AuthenticationStateUser = authSatate.User;

        return authSatate;
    }

    /// <summary>
    /// Gets the current authentication state user
    /// </summary>
    /// <returns>The current user's claims principal</returns>
    public async Task<ClaimsPrincipal> GetAuthenticationStateUserAsync()
    {
        var authState = await GetAuthenticationStateAsync();

        var authStateUser = authState.User;

        return authStateUser;
    }

    /// <summary>
    /// Change the authentication state
    /// </summary>
    public async Task StateChangedAsync()
    {
        // verify the current authentication state
        var authState = Task.FromResult(await GetAuthenticationStateAsync());

        // update the authentication state
        base.NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        // return empty credentials
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        // update the authentication state
        base.NotifyAuthenticationStateChanged(authState);
    }

    private async Task<IEnumerable<Claim>> GetClaimsFromJwtAsync(string jwt)
    {
        // instantiates the list of claims to return
        var output = new List<Claim>();

        // separate the token string
        var payload = jwt.Split('.')[1];

        // get the byte array from the token string
        var jsonBytes = Conversion.ParseBase64WithoutPadding(payload);

        // get the key value pairs for claims from the byte array
        var claimsDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);


        if (claimsDictionary is not null)
        {
            // get all the role claim types from the dictionary
            claimsDictionary.TryGetValue(ClaimTypes.Role, out var roles);

            if (roles is not null)
            {
                // if roles start with '[' then this is an array
                if (roles.ToString()!.Trim().StartsWith("["))
                {
                    // get roles as string array
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                    // add the array to the claims list
                    output.AddRange(parsedRoles!.Select(role => new Claim(ClaimTypes.Role, role)));
                }
                else
                {
                    // add the role to the claims list
                    output.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                }

                // removed roles from dictionary to prevent adding duplicates
                claimsDictionary.Remove(ClaimTypes.Role);
            }

            // get all the permission claim types from the dictionary
            claimsDictionary.TryGetValue(HubClaimTypes.Permission, out var permissions);

            if (permissions is not null)
            {
                // if permissions start with '[' then this is an array
                if (permissions.ToString()!.Trim().StartsWith("["))
                {
                    // get roles as string array
                    var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString()!);

                    // add the array to the claims list
                    output.AddRange(parsedPermissions!.Select(permission => new Claim(HubClaimTypes.Permission, permission)));
                }
                else
                {
                    // add the permission to the claims list
                    output.Add(new Claim(HubClaimTypes.Permission, permissions.ToString()!));
                }

                // removed permissions from dictionary to prevent adding duplicates
                claimsDictionary.Remove(HubClaimTypes.Permission);
            }

            // add all the remaining claims to the claims list
            output.AddRange(claimsDictionary.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
        }

        if (TryGetUserDetails(await _storageService.GetAsync<string>(StorageKey.Identity.UserDetail), out var details))
        {
            output.AddRange(details);
        }

        return output;
    }

    private static bool TryGetUserDetails(string input, out IEnumerable<Claim> claims)
    {
        claims = Enumerable.Empty<Claim>();

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        try
        {
            var data = JsonSerializer.Deserialize<UserDetailResponse>(input);
            if (data is not null)
            {
                claims = new List<Claim>()
                {
                    new Claim(HubClaimTypes.Avatar, data.ProfileImage.Image.Url),
                    new Claim(HubClaimTypes.FirstName, data.FirstName),
                    new Claim(HubClaimTypes.LastName, data.LastName),
                    new Claim(HubClaimTypes.UserName, data.Username),
                    new Claim(HubClaimTypes.Email, data.Email),
                };
            }

            return claims.Any();
        }
        catch (Exception)
        {
            return false;
        }
    }
}
