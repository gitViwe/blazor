namespace Shared.Contract;

public class PaginatedRequest : IPaginatedRequest
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public string ToQueryParameterString() => $"?CurrentPage={CurrentPage}&PageSize={PageSize}";
}
