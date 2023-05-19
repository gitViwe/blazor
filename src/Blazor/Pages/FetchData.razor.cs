namespace Blazor.Pages;

public partial class FetchData : IDisposable
{
    public void Dispose()
    {
        HttpInterceptorManager.DisposeEvent();
        GC.SuppressFinalize(this);
    }

    protected override Task OnInitializedAsync()
    {
        HttpInterceptorManager.RegisterEvent();
        return Task.CompletedTask;
    }
}
