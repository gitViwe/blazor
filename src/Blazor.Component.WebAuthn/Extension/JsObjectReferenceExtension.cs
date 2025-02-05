namespace Blazor.Component.WebAuthn.Extension;

internal static class JsObjectReferenceExtension
{
    public static ValueTask<bool> IsWebAuthnSupportedAsync(this IJSObjectReference jsObjectReference)
        => jsObjectReference.InvokeAsync<bool>("isWebAuthnPossible");
    
    public static ValueTask<AuthenticatorAttestationRawResponse> CreateCredentialsAsync(
        this IJSObjectReference jsObjectReference,
        CredentialCreateOptions options)
        => jsObjectReference.InvokeAsync<AuthenticatorAttestationRawResponse>("createCredentials", options);
    
    public static ValueTask<AuthenticatorAssertionRawResponse> VerifyAsync(
        this IJSObjectReference jsObjectReference,
        AssertionOptions options)
        => jsObjectReference.InvokeAsync<AuthenticatorAssertionRawResponse>("verify", options);
}