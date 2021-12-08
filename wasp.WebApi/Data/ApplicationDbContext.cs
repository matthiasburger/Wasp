using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using wasp.Core;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.SurrogateKeyGenerator;

using Index = wasp.WebApi.Data.Models.Index;

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
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<DataArea> DataAreas { get; set; } = null!;
        public DbSet<DataField> DataFields { get; set; } = null!;
        public DbSet<PrimaryKey> PrimaryKeys { get; set; } = null!;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataItem>().HasKey(x => new { x.Id, x.DataTableId });

            modelBuilder.Entity<Relation>()
                .HasKey(x => new { x.IndexId, x.KeyDataItemId, x.KeyDataTableId })
                .IsClustered(false);
            
            modelBuilder.Entity<Relation>()
                .HasOne(x => x.KeyDataItem)
                .WithMany(x=>x.KeyRelations);
            
            modelBuilder.Entity<Relation>()
                .HasOne(x=>x.ReferenceDataItem)
                .WithMany(x=>x.ReferenceRelations)
                .HasForeignKey(x=>new{x.ReferenceDataItemId, x.ReferenceDataTableId});
        }

        private static async Task _setSurrogatePrimaryKeysAsync(object entity)
        {
            if (entity is IKeyProducible p)
                await p.SetKey();
        }
        private static void _onAdd(object entity)
        {
            AsyncSyncRunner.RunSync(()=>_setSurrogatePrimaryKeysAsync(entity));
        }
        
        private static async Task _onAddAsync(object entity)
        {
            await _setSurrogatePrimaryKeysAsync(entity);
        }

        public override EntityEntry Add(object entity)
        {
            _onAdd(entity);
            
            return base.Add(entity);
        }

        public override async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = new ())
        {
            await _onAddAsync(entity);

            return await base.AddAsync(entity, cancellationToken);
        }

        public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new ())
        {
            await _onAddAsync(entity);

            return await base.AddAsync(entity, cancellationToken);
        }
    }
}