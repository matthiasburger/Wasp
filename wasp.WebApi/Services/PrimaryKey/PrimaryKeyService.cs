using System;
using System.Threading.Tasks;

using IronSphere.Extensions.Reflection;

using Microsoft.EntityFrameworkCore;

using wasp.WebApi.Data;

namespace wasp.WebApi.Services.PrimaryKey
{
    public class PrimaryKeyService : IPrimaryKeyService
    {
        private readonly ApplicationDbContext _dbContext;

        public PrimaryKeyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Data.Models.PrimaryKey?> GetCurrentId(Type type)
        {
            return await _dbContext.PrimaryKeys
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.TableName == type.Name);
        }

        public async Task SetCurrentId<TType>(Type type, TType nextKey)
        {
            Data.Models.PrimaryKey? key = await _dbContext.PrimaryKeys
                .SingleOrDefaultAsync(x => x.TableName == type.Name);

            if (key == null)
            {
                key = new Data.Models.PrimaryKey
                {
                    TableName = type.GetReadableName(),
                    CurrentId = nextKey?.ToString()!
                };
                await _dbContext.AddAsync(key);
            }
            else
            {
                key.CurrentId = nextKey?.ToString()!;
                _dbContext.Update(key);
            }
            
            await _dbContext.SaveChangesAsync();
        }
    }
}