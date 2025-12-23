namespace Blazor.Shared.Constant;

public static class HubLazyAssembly
{
    public static class WebAuthentication
    {
        public const string Path = BlazorPage.Security.WebAuthn;
        public static readonly IEnumerable<string> Assemblies = ["Blazor.Component.WebAuthn.dll", "Fido2.Models.dll"];
    }
}