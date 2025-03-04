import scrollToElement from "./hub-component/hub-anchor-navigation";
import {
    AuthenticatorAttestationRawResponse,
    AuthenticatorAssertionRawResponse,
    isWebAuthnPossible,
    createCredentials,
    verify
} from "./hub-web-authentication/hub-web-authentication";

declare global {
    // extend Window to add a custom property
    interface Window {
        HubComponent: HubComponent;
        HubWebAuthentication: HubWebAuthentication;
    }

    interface HubComponent {
        ScrollToElement(elementId: string): void
    }

    interface HubWebAuthentication {
        IsWebAuthnPossible(): boolean,
        CreateCredentials(options: PublicKeyCredentialCreationOptions): Promise<AuthenticatorAttestationRawResponse>,
        Verify(options: PublicKeyCredentialRequestOptions): Promise<AuthenticatorAssertionRawResponse>
    }
}

// assign functions
window.HubComponent = {
    ScrollToElement: scrollToElement
};

window.HubWebAuthentication = {
    IsWebAuthnPossible: isWebAuthnPossible,
    CreateCredentials: createCredentials,
    Verify: verify,
};