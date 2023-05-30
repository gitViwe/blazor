namespace Blazor.Pages;

public partial class FetchData : IDisposable
{
    public void Dispose()
    {
        HttpInterceptorService.DisposeEvent();
        LocationChangedInterceptorService.DisposeEvent();
        GC.SuppressFinalize(this);
    }

    protected override Task OnInitializedAsync()
    {
        HttpInterceptorService.RegisterEvent();
        LocationChangedInterceptorService.RegisterEvent();
        return Task.CompletedTask;
    }
}
