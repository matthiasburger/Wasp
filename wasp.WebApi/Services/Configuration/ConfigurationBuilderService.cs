using System;
using System.IO;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Microsoft.Extensions.Configuration;
using wasp.Core;

namespace wasp.WebApi.Services.Configuration
{
    public class ConfigurationBuilderService : IConfigurationBuilderService
    {
        public IConfiguration GetPlatformAgnosticConfig(string[]? cliArgs = null)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile(@"appsettings.json", false, true);
            configurationBuilder.AddJsonFile(@"appsettings.{System.Environment.GetEnvironmentVariable(""ASPNETCORE_ENVIRONMENT"") ?? ""Production""}.json", true, true);
            configurationBuilder.AddJsonFile(_ensureDeviceConfigExists(), false, true);
            configurationBuilder.AddJsonFile("secrets.json", true, true);
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddCommandLine(cliArgs ?? Array.Empty<string>());
            
            return configurationBuilder.Build();
        }

        private string _ensureDeviceConfigExists()
        {
            string computerName = DeviceInformation.GetComputerName();
            string path = $@"appsettings.{computerName}.json";
            
            if (!File.Exists(path))
            {
                using FileStream fileStream = File.Create(path);
                fileStream.Write($"{{{System.Environment.NewLine}}}".GetBytes());
            }
            
            return path;
        }
    }
}