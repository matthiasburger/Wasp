using Microsoft.Extensions.Configuration;

namespace wasp.WebApi.Services.Configuration
{
    public interface IConfigurationService
    {
        IConfiguration GetPlatformAgnosticConfig(string[] cliArgs = null);
    }
}