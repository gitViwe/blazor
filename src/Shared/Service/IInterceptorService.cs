namespace Shared.Service;

public interface IInterceptorService : IDisposable
{
    /// <summary>
    /// Registers the HTTP intercept events
    /// </summary>
    void RegisterEvent();
}
