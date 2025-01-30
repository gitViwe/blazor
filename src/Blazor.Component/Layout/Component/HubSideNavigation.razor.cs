namespace Blazor.Component.Layout.Component;

public partial class HubSideNavigation : ComponentBase
{
    [Inject]
    public required IWebAssemblyHostEnvironment Environment { get; init; }
}