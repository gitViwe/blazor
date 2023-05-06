using gitViwe.Shared;
using gitViwe.Shared.Model;

namespace Blazor.Presentation.Page.FetchData;

public partial class HubSuperHeroTable
{
    [Parameter, EditorRequired] public Func<IPaginatedRequest, Task<PaginatedResponse<SuperHeroResponse>>> PaginatedDataSource { get; set; }

    private bool _dense = true;
    private bool _striped = true;
    private bool _bordered = true;

    private async Task<TableData<SuperHeroResponse>> GetHeroesToTableDataAsync(TableState state)
    {
        var result = await PaginatedDataSource(new PaginatedRequest { CurrentPage = state.Page + 1, PageSize = state.PageSize });

        return new TableData<SuperHeroResponse>() { TotalItems = result.TotalCount, Items = result.Data };
    }

    private static Color GetColor(long value)
    {
        if (value > 69)
        {
            return Color.Success;
        }

        if (value > 39)
        {
            return Color.Warning;
        }

        return Color.Error;
    }
}
