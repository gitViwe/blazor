using Microsoft.JSInterop;

namespace Blazor.Infrastructure.Service;

internal class LocalStorageService : IStorageService
{
    private readonly IJSRuntime _runtime;

    public LocalStorageService(IJSRuntime runtime)
    {
        _runtime = runtime;
    }
    public ValueTask<TResult> GetAsync<TResult>(string key, CancellationToken token = default)
    {
        // run a JavaScript function to get item based on the 'key'
        return _runtime.InvokeAsync<TResult>("localStorage.getItem", token, key);
    }

    public ValueTask RemoveAsync(string key, CancellationToken token = default)
    {
        // run a JavaScript function to delete item
        return _runtime.InvokeVoidAsync("localStorage.removeItem", token, key);
    }

    public ValueTask SetAsync<TData>(string key, TData data, CancellationToken token = default)
    {
        // run a JavaScript function to save item
        return _runtime.InvokeVoidAsync("localStorage.setItem", token, key, data);
    }
}
