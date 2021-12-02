using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace wasp.WebApi.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public IConfiguration GetPlatformAgnosticConfig(string[]? cliArgs = null)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile(@"appsettings.json", false, true);
            configurationBuilder.AddJsonFile(@"appsettings.{System.Environment.GetEnvironmentVariable(""ASPNETCORE_ENVIRONMENT"") ?? ""Production""}.json", true, true);
            configurationBuilder.AddJsonFile("secrets.json", true, true);
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddCommandLine(cliArgs ?? Array.Empty<string>());
            
            return configurationBuilder.Build();
        }
    }
}