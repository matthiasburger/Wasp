using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using wasp.WebApi.Data.Models;
using wasp.WebApi.Services.Configuration;
using wasp.WebApi.Services.Environment;

namespace wasp.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEnvironmentDiscovery _environmentDiscovery;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory,
            IEnvironmentDiscovery environmentDiscovery) : base(options)
        {
            _loggerFactory = loggerFactory;
            _environmentDiscovery = environmentDiscovery;
        }
        
        public DbSet<DataTable> DataTables { get; set; }
        public DbSet<DataItem> DataItems { get; set; }
        public DbSet<Index> Indexes { get; set; }
        public DbSet<Relation> Relations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataItem>().HasKey(x => new { x.Id, x.DataTableId });

            modelBuilder.Entity<Relation>()
                .HasKey(x => new { x.IndexId, x.KeyDataItemId, x.KeyDataTableId, x.ReferenceDataItemId, x.ReferenceDataTableId })
                .IsClustered(false);
            
            modelBuilder.Entity<Relation>()
                .HasOne(x => x.KeyDataItem)
                .WithMany(x=>x.KeyRelations)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Relation>()
                .HasOne(x => x.ReferenceDataItem)
                .WithMany(x=>x.ReferenceRelations)
                .HasForeignKey(x=>new{x.ReferenceDataItemId, x.ReferenceDataTableId})
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
    
    public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationService configurationService = new ConfigurationService();
            IConfiguration configuration = configurationService.GetPlatformAgnosticConfig();

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("WaspSqlServerConnectionString"));

            return new ApplicationDbContext(optionsBuilder.Options, new LoggerFactory(), new EnvironmentDiscovery());
        }
    }
}