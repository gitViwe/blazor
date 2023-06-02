namespace Shared.Service;

/// <summary>
/// Intercepts the location changed event an performs an action
/// </summary>
public interface ILocationChangedInterceptorService
{
    /// <summary>
    /// Subscribes to the LocationChanged event
    /// </summary>
    void RegisterEvent();

    /// <summary>
    /// Unsubscribes from the LocationChanged event
    /// </summary>
    void DisposeEvent();
}
