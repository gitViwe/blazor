namespace Blazor.Component.WebAuthn.Model;

public sealed class HubAuthenticatorAttestationRawResponse
{
    [JsonConverter(typeof(Base64UrlConverter))]
    [JsonPropertyName("id")]
    public required byte[] Id { get; init; }

    [JsonConverter(typeof(Base64UrlConverter))]
    [JsonPropertyName("rawId")]
    public required byte[] RawId { get; init; }

    [JsonPropertyName("type")]
    public PublicKeyCredentialType? Type { get; init; }

    [JsonPropertyName("response")]
    public required AuthenticatorAttestationRawResponse.ResponseData Response { get; init; }

    [JsonPropertyName("extensions")]
    public required AuthenticationExtensionsClientOutputs Extensions { get; init; }

    public sealed class ResponseData
    {
        [JsonConverter(typeof(Base64UrlConverter))]
        [JsonPropertyName("attestationObject")]
        public required byte[] AttestationObject { get; init; }

        [JsonConverter(typeof(Base64UrlConverter))]
        [JsonPropertyName("clientDataJSON")]
        public required byte[] ClientDataJson { get; init; }
        
        [JsonPropertyName("transports")]
        public required AuthenticatorTransport[] Transports { get; init; }
    }
}