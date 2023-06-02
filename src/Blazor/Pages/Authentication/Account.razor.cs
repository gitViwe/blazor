namespace Blazor.Pages.Authentication;

public partial class Account : IDisposable
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
            HttpInterceptorService.RegisterEvent();
            LocationChangedInterceptorService.RegisterEvent();
        }
        return base.OnAfterRenderAsync(firstRender);
    }
}
