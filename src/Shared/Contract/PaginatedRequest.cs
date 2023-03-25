namespace Shared.Contract;

public class PaginatedRequest
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public string ToQueryParameterString() => $"?CurrentPage={CurrentPage}&PageSize={PageSize}";
}
