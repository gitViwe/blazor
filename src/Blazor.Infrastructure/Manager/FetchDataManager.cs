﻿using gitViwe.Shared.Model;

namespace Blazor.Infrastructure.Manager;

internal class FetchDataManager : IFetchDataManager
{
    private readonly HttpClient _httpClient;

    public FetchDataManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResponse<SuperHeroResponse>> GetHeroesAsync(IPaginatedRequest request)
    {
        var result = await _httpClient.GetAsync(Shared.Route.AuthenticationAPI.AccountEndpoint.SuperHero + request.ToQueryParameterString());

        if (result.IsSuccessStatusCode)
        {
            return await result.ToPaginatedResponseAsync<SuperHeroResponse>();
        }

        return PaginatedResponse<SuperHeroResponse>.Fail();
    }
}
