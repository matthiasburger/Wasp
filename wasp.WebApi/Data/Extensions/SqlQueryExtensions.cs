using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace wasp.WebApi.Data.Extensions
{
    public static class SqlQueryExtensions
    {
        public static async Task<IList<T>> SqlQuery<T>(this DbContext db, string sql, params object[] parameters) where T : class
        {
            await using (ContextForQueryType<T> db2 = new(db.Database.GetDbConnection()))
            {
                // share the current database transaction, if one exists
                IDbContextTransaction? transaction = db.Database.CurrentTransaction;
                if (transaction != null)
                    await db2.Database.UseTransactionAsync(transaction.GetDbTransaction());
                return await db2.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
            }
        }

        public static async Task<IList<T>> SqlQuery<T>(this DbContext db, Func<T> anonType, string sql, params object[] parameters) where T : class
            => await SqlQuery<T>(db, sql, parameters);

        private class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection connection;

            public ContextForQueryType(DbConnection connection)
            {
                this.connection = connection;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // switch on the connection type name to enable support multiple providers
                // var name = con.GetType().Name;
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());

                base.OnConfiguring(optionsBuilder);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey();
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}