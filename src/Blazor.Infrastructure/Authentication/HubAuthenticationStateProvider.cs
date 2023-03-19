using Microsoft.AspNetCore.Components.Authorization;
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
        var savedToken = await _storageService.GetAsync<string>(StorageKey.Local.AuthToken);

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            // return empty credentials if no token found
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // get the authentication state using the saved token
        var authSatate = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));

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
    /// Change the authentication state when user logs out
    /// </summary>
    public void MarkUserAsLoggedOut()
    {
        // create blank user claims principal
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

        // create authentication state using blank user
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));

        // update the authentication state
        base.NotifyAuthenticationStateChanged(authState);
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

    private static IEnumerable<Claim> GetClaimsFromJwt(string jwt)
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

        return output;
    }
}
