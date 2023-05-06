namespace Shared.Contract.ProblemDetail;

public class ValidationProblemDetails : IValidationProblemDetails
{
    public string? TraceId { get; init; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public IDictionary<string, object?> Extensions { get; set; } = new Dictionary<string, object?>();
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}