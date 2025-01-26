namespace Blazor.Component.Page.Home.Component;

public partial class SwaggerProjectSection : ComponentBase
{
    [Inject]
    public required IConfiguration Configuration { get; init; }

    private string SwaggerUiPath => Configuration.GetSwaggerUri();
}