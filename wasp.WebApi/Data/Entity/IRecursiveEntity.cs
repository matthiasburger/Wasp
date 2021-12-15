using System.Collections.Generic;

namespace wasp.WebApi.Data.Entity
{
    public interface IRecursiveEntity<TEntity, TType> where TEntity : IEntity<TType>
    {
        TEntity Parent { get; set; }
        ICollection<TEntity> Children { get; set; }
    }
}