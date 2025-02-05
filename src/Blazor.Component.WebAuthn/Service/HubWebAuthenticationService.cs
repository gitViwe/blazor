namespace Blazor.Component.WebAuthn.Service;

internal static class HubWebAuthenticationService
{
    private static readonly JsonSerializerOptions JsonOptions = new Serializer.FidoBlazorSerializerContext().Options;
    
    public static async Task<IResponse> RegisterAsync(
        string displayName,
        ILogger logger,
        HttpClient httpClient,
        IConfiguration configuration,
        IJSObjectReference jsObjectReference,
        CancellationToken cancellationToken)
    {
        // Get options from server
        var credentialOptionsResponse = await httpClient.GetAsync(configuration.GetWebAuthnCredentialOptionsUri(displayName), cancellationToken);

        if (credentialOptionsResponse.IsSuccessStatusCode is false)
        {
            return Response.Fail("No options received");
        }
        
        var options = await credentialOptionsResponse.Content.ReadFromJsonAsync<CredentialCreateOptions>(JsonOptions, cancellationToken);

        if (options is null)
        {
            return Response.Fail("No options received");
        }

        try
        {
            // Present options to user and get response
            var credential = await jsObjectReference.CreateCredentialsAsync(options);
            var b = JsonSerializer.Serialize(credential, JsonOptions);
            // Send response to server
            var createCredentialsResponse = await httpClient.PutAsJsonAsync(configuration.GetWebAuthnCreateCredentialsUri(options.User.Name), credential, JsonOptions, cancellationToken);
            
            return createCredentialsResponse.IsSuccessStatusCode
                ? Response.Success("Successfully created credentials")
                : Response.Fail("Failed to create credentials");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to create credentials");
            var message = "Failed to create credentials." + (options.ExcludeCredentials?.Count > 0 ? " (You may have already registered this device)" : string.Empty);
            return Response.Fail(message);
        }
    }

    public static async Task<IResponse> LoginAsync(
        ILogger logger,
        HttpClient httpClient,
        IConfiguration configuration,
        IJSObjectReference jsObjectReference,
        CancellationToken cancellationToken)
    {
        var assertionOptionsResponse = await httpClient.GetAsync(configuration.GetWebAuthnAssertionOptionsUri(), cancellationToken);

        if (assertionOptionsResponse.IsSuccessStatusCode is false)
        {
            return Response.Fail("No options received");
        }
        
        var options = await assertionOptionsResponse.Content.ReadFromJsonAsync<AssertionOptions>(JsonOptions, cancellationToken);

        if (options is null)
        {
            return Response.Fail("No options received");
        }
        
        try
        {
            // Present options to user and get response (usernameless users will be asked by their authenticator, which credential they want to use to sign the challenge)
            var assertion = await jsObjectReference.VerifyAsync(options);
            
            // Send response to server
            var assertionResponse = await httpClient.PostAsJsonAsync(configuration.GetWebAuthnAssertionUri(), assertion, JsonOptions, cancellationToken);

            return assertionResponse.IsSuccessStatusCode
                ? Response.Success("Successfully created token")
                : Response.Fail("Failed to create token");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to create token");
            return Response.Fail("Failed to create token");
        }
    }
}