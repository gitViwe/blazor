namespace Shared.Service;

/// <summary>
/// Intercepts HTTP requests
/// </summary>
public interface IHttpInterceptorService
{
    /// <summary>
    /// We remove the BeforeSendAsync and AfterSendAsync event subscription from the event handler
    /// </summary>
    void DisposeEvent();

    /// <summary>
    /// Registering an event to the BeforeSendAsync and AfterSendAsync event handler.<br/>
    /// This means, before the HTTP request is sent, it is intercepted and the an event is fired<br></br>
    /// and after the HTTP request is sent, it is intercepted and the an event is fired
    /// </summary>
    /// <param name="interceptBeforeHttpTask">The method to invoke after intercepting the BeforeSendAsync event</param>
    /// <param name="interceptAfterHttpTask">The method to invoke after intercepting the AfterSendAsync event</param>
    void RegisterEvent(Func<Task>? interceptBeforeHttpTask = null, Func<IValidationProblemDetails, Task>? interceptAfterHttpTask = null);
}
