using gitViwe.Shared.Abstraction;

namespace Shared.Contract.ProblemDetail;

public class DefaultProblemDetails : IDefaultProblemDetails
{
    public string? TraceId { get; init; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>();
}
