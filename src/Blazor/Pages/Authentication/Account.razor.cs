namespace Blazor.Pages.Authentication;

public partial class Account : IDisposable
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
