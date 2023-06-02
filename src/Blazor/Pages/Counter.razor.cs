namespace Blazor.Pages;

public partial class Counter : IDisposable
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
