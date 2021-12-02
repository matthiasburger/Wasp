using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using wasp.WebApi.Data.Models;

namespace wasp.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public DbSet<DataTable> DataTables { get; set; } = null!;
        public DbSet<DataItem> DataItems { get; set; } = null!;
        public DbSet<Index> Indexes { get; set; } = null!;
        public DbSet<Relation> Relations { get; set; } = null!;

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
}