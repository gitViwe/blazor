namespace Blazor.Shared.Interface;

public interface IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts { get; }
}