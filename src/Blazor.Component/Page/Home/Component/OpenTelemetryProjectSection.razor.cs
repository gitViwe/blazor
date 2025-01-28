namespace Blazor.Component.Page.Home.Component;

public partial class OpenTelemetryProjectSection : ComponentBase
{
    [Inject]
    public required IConfiguration Configuration { get; init; }
    private string SeqUiUrl => Configuration.GetSeqUiUri();
    private string JaegerUiUrl => Configuration.GetJaegerUiUri();
    private string GrafanaDashboardUrl => Configuration.GetGrafanaDashboardUri();
    private string ToolTipText
        => string.IsNullOrWhiteSpace(SeqUiUrl) || string.IsNullOrWhiteSpace(JaegerUiUrl) || string.IsNullOrWhiteSpace(GrafanaDashboardUrl)
            ? "Run via Docker to enable these" : string.Empty;
    private string GrafanaCredentialsToolTipText
        => string.IsNullOrWhiteSpace(ToolTipText) ? "username: admin | password: admin" : string.Empty;
}