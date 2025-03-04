namespace Blazor.Section.Home;

public partial class GrapQlProjectSection : ComponentBase
{
    [CascadingParameter]
    public required CascadingHubFeatureManagerContext HubFeatureManagerContext { get; init; }
    
    [Inject]
    public required IConfiguration Configuration { get; init; }
}