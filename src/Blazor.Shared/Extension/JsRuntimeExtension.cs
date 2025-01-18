using Microsoft.JSInterop;

namespace Blazor.Shared.Extension;

public static class JsRuntimeExtension
{
    public static ValueTask OpenNewTabAsync(this IJSRuntime jsRuntime, string url) => jsRuntime.InvokeVoidAsync("open", url, "_blank");
}