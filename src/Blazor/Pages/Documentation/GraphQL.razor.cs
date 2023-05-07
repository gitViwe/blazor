using Shared.Constant;

namespace Blazor.Pages.Documentation;

public partial class GraphQL
{
    private static string GetGraphQLEndpoint(IConfiguration configuration)
    {
        return configuration[ConfigurationKey.API.ServerUrl] + "/graphql";
    }
}
