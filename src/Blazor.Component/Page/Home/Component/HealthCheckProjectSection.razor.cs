namespace Blazor.Component.Page.Home.Component;

public partial class HealthCheckProjectSection : ComponentBase
{
    [Inject]
    public required IConfiguration Configuration { get; init; }

    private string HealthCheckPath => Configuration.GetHealthCheckUri();
}