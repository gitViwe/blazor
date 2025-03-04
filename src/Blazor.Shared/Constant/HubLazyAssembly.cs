namespace Blazor.Shared.Constant;

public static class HubLazyAssembly
{
    public class WebAuthentication
    {
        public static readonly IEnumerable<string> Assemblies = ["Blazor.Component.WebAuthn.dll", "Fido2.Models.dll"];
    }
}