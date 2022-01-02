using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Serilog;

using wasp.WebApi.Services.Configuration;
using wasp.WebApi.Services.Environment;

namespace wasp.WebApi
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Program
    {
        private static readonly IEnvironmentDiscovery EnvironmentDiscovery = new EnvironmentDiscovery();
        private static readonly IConfigurationBuilderService ConfigurationService = new ConfigurationBuilderService();
        
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(ConfigurationService.GetPlatformAgnosticConfig(args))
                .Enrich.FromLogContext()
                .CreateLogger();
            
            Log.Information($"Starting {nameof(wasp)} web host...");
            CreateWebHostBuilder(args).Build().Run();
            return 0;
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (EnvironmentDiscovery.IsDocker)
                Log.Information("Running inside Docker");

            return WebHost
                .CreateDefaultBuilder(args)
                .UseConfiguration(ConfigurationService.GetPlatformAgnosticConfig(args))
                .UseUrls(EnvironmentDiscovery.IsDocker ? "http://*:80" : "http://localhost:8000")
                .UseStartup<Startup>()
                .UseSerilog();
        }
    }
}
