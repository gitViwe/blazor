namespace Blazor.Pages;

public partial class Home
{
    [Inject]
    public required IJSRuntime JsRuntime { get; init; }

    private async ValueTask OpenNewTabAsync(string url) => await JsRuntime.InvokeVoidAsync("open", url, "_blank");
}