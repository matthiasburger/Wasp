using System;
using System.Threading.Tasks;

using IronSphere.Extensions.Reflection;

using wasp.WebApi.Data.Models;
using wasp.WebApi.Exceptions;
using wasp.WebApi.Services.PrimaryKey;
using wasp.WebApi.Services.StaticServiceProvider;

namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    public abstract class SurrogateBaseEntity<TType, TKeyGenerator> : Entity.Entity<TType>, IKeyProducible
        where TKeyGenerator : ISurrogateKeyGenerator<TType>, new()
    {
        private readonly IPrimaryKeyService _primaryKeyService;

        protected SurrogateBaseEntity()
        {
            _primaryKeyService = ServiceLocator.ServiceProvider.GetService<IPrimaryKeyService>() 
                                 ?? throw new CallDependencyInjectionException($"could not locate a service for {nameof(IPrimaryKeyService)}");
        }

        public async Task SetKey()
        {
            TKeyGenerator generator = await _createInstanceFromTable();
            TType nextKey = generator.GetNextKey();

            if (nextKey is not null)
            {
                Id = nextKey;
                await _primaryKeyService.SetCurrentId(GetType(), nextKey);
            }
        }

        private async Task<TKeyGenerator> _createInstanceFromTable()
        {
            PrimaryKey? primaryKeyEntity = await _primaryKeyService.GetCurrentId(GetType());
            
            if(primaryKeyEntity?.CurrentId is TType value)
                return new TKeyGenerator
                {
                    CurrentValue = value
                };

            throw new Exception($"Type {primaryKeyEntity?.CurrentId.GetType().GetReadableName()} with value [{primaryKeyEntity?.CurrentId}] is not from type {typeof(TType).GetReadableName()}");
        }
    }
}
