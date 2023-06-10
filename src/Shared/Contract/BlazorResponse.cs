namespace Shared.Contract;

public interface IBlazorResponse : IResponse
{
    string Data { get; }
}

public class BlazorResponse : IBlazorResponse
{
    public string Data { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}
