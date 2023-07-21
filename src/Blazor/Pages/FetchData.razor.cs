namespace Blazor.Pages;

public partial class FetchData : IDisposable
{
    public void Dispose()
    {
        HttpInterceptorService.DisposeEvent();
        LocationChangedInterceptorService.DisposeEvent();
        GC.SuppressFinalize(this);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            HttpInterceptorService.RegisterEvent(interceptAfterHttpTask: problem => Snackbar.AddWarningAsync(problem.Detail));
            LocationChangedInterceptorService.RegisterEvent();
        }
        return base.OnAfterRenderAsync(firstRender);
    }
}
