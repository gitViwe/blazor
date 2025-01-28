namespace Blazor.Component.Page.Home.Component;

public partial class GrapQlProjectSection : ComponentBase
{
    [Inject]
    public required IConfiguration Configuration { get; init; }

    private string GraphQlPath => Configuration.GetGraphQlUri();
}