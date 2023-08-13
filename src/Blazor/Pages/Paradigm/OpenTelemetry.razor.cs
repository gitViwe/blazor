namespace Blazor.Pages.Paradigm;

public partial class OpenTelemetry : IDisposable
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
    private static string GetSwaggerEndpoint(IConfiguration configuration)
    {
        return string.Empty;
    }
}

