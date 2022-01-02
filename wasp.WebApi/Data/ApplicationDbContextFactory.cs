using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using wasp.WebApi.Services.Configuration;

namespace wasp.WebApi.Data
{
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by EF Core Tools")]
    public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationBuilderService configurationService = new ConfigurationBuilderService();
            IConfiguration configuration = configurationService.GetPlatformAgnosticConfig();

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("WaspSqlServerConnectionString"));

            return new ApplicationDbContext(optionsBuilder.Options, new LoggerFactory());
        }
    }
}