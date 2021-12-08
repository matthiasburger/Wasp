using System;
using System.Threading.Tasks;

namespace wasp.WebApi.Services.PrimaryKey
{
    public interface IPrimaryKeyService
    {
        Task<Data.Models.PrimaryKey?> GetCurrentId(Type type);
        Task SetCurrentId<TType>(Type type, TType nextKey);
    }
}