using gitViwe.Shared.Abstraction;

namespace Shared.Contract.ProblemDetail;

/// <summary>
/// Extends on <seealso cref="IDefaultProblemDetails"/> to add <seealso cref="Errors"/>
/// </summary>
public interface IValidationProblemDetails : IDefaultProblemDetails
{
    /// <summary>
    /// Gets the validation errors associated with this instance of HttpValidationProblemDetails
    /// </summary>
    IDictionary<string, string[]> Errors { get; }
}

public class ValidationProblemDetails : IValidationProblemDetails
{
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
    public string? TraceId { get; init; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>();
}