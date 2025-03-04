namespace Blazor.Shared.Extension;

public static class JsRuntimeExtension
{
    public static ValueTask OpenNewTabAsync(this IJSRuntime jsRuntime, string url, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("open", cancellationToken, url, "_blank");
    
    public static ValueTask ImportJsModuleAsync(this IJSRuntime jsRuntime, string modulePath, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("import", cancellationToken, modulePath);
    
    public static ValueTask<IJSObjectReference> ImportJsModuleReferenceAsync(this IJSRuntime jsRuntime, string modulePath, CancellationToken cancellationToken)
        => jsRuntime.InvokeAsync<IJSObjectReference>("import", cancellationToken, modulePath);
    
    public static ValueTask<T> LocalStorageGetAsync<T>(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => jsRuntime.InvokeAsync<T>("localStorage.getItem", cancellationToken, key);
    
    public static ValueTask LocalStorageRemoveAsync(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
    
    public static ValueTask LocalStorageSetAsync<T>(this IJSRuntime jsRuntime, string key, T data, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data);
    
    public static ValueTask<T> SessionStorageGetAsync<T>(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => jsRuntime.InvokeAsync<T>("sessionStorage.getItem", cancellationToken, key);
    
    public static ValueTask SessionStorageRemoveAsync(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", cancellationToken, key);
    
    public static ValueTask SessionStorageSetAsync<T>(this IJSRuntime jsRuntime, string key, T data, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("sessionStorage.setItem", cancellationToken, key, data);
}