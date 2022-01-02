using Microsoft.Extensions.Configuration;

namespace wasp.WebApi.Services.Configuration
{
    public interface IConfigurationBuilderService
    {
        IConfiguration GetPlatformAgnosticConfig(string[]? cliArgs = null);
    }
}