namespace Blazor.Pages.Authentication;

public partial class Register : IDisposable
{
    public void Dispose()
    {
        LocationChangedInterceptorService.DisposeEvent();
        GC.SuppressFinalize(this);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            LocationChangedInterceptorService.RegisterEvent();
        }
        return base.OnAfterRenderAsync(firstRender);
    }
}
