var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHubComponentServices()
    .AddHubWebAuthenticationServices()
    .AddGatewayClient()
    .RegisterClientAuthorization();

await builder.Build().RunAsync();