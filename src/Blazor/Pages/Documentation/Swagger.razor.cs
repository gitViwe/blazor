using Shared.Constant;

namespace Blazor.Pages.Documentation;

public partial class Swagger
{
    private static string GetSwaggerEndpoint(IConfiguration configuration)
    {
        return configuration[ConfigurationKey.API.ServerUrl] + "/swagger";
    }
}
