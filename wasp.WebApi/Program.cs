using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using wasp.WebApi.Services.Configuration;
using wasp.WebApi.Services.Environment;

namespace wasp.WebApi
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Program
    {
        private static readonly IEnvironmentDiscovery EnvironmentDiscovery = new EnvironmentDiscovery();
        private static readonly IConfigurationService ConfigurationService = new ConfigurationService();
        
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (EnvironmentDiscovery.IsDocker)
            {
                Console.WriteLine("Running inside Docker");
            }

            return WebHost
                .CreateDefaultBuilder(args)
                .UseConfiguration(ConfigurationService.GetPlatformAgnosticConfig(args))
                .UseUrls(EnvironmentDiscovery.IsDocker ? "http://*:80" : "http://localhost:8000")
                .UseStartup<Startup>();
        }
    }
}
