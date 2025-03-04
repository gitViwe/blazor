using Blazor.Client;
using Blazor.Component;
using Blazor.Component.WebAuthn;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddMudServices()
    .AddHubComponentServices()
    .AddHubWebAuthenticationServices()
    .AddGatewayClient();

await builder.Build().RunAsync();