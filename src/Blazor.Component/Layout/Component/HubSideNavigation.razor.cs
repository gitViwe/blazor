namespace Blazor.Component.Layout.Component;

public partial class HubSideNavigation : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts => new();
    
    [CascadingParameter]
    public required CascadingHubFeatureManagerContext HubFeatureManagerContext { get; init; }

    [Inject]
    public required IWebAssemblyHostEnvironment Environment { get; init; }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}