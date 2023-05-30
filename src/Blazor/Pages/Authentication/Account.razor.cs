namespace Blazor.Pages.Authentication;

public partial class Account : IDisposable
{
    public void Dispose()
    {
        HttpInterceptorService.DisposeEvent();
        GC.SuppressFinalize(this);
    }

    protected override Task OnInitializedAsync()
    {
        HttpInterceptorService.RegisterEvent();
        return Task.CompletedTask;
    }
}
