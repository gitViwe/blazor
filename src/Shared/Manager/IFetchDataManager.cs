using Shared.Contract;
using Shared.Contract.FetchData;

namespace Shared.Manager;

/// <summary>
/// Fetch Data client manager
/// </summary>
public interface IFetchDataManager
{
    /// <summary>
    /// Gets all the heroes from the API endpoint
    /// </summary>
    /// <param name="request">The current page and number of items being displayed</param>
    /// <returns>A <see cref="PaginatedResponse{SuperHeroResponse}"/> where the data is a collection of <see cref="SuperHeroResponse"/> objects</returns>
    Task<PaginatedResponse<SuperHeroResponse>> GetHeroesAsync(PaginatedRequest request);
}
