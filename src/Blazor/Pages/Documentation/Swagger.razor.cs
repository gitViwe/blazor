using Shared.Constant;

namespace Blazor.Pages.Documentation;

public partial class Swagger : IDisposable
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
        return configuration[ConfigurationKey.API.ServerUrl] + "/swagger";
    }
}
