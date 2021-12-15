using System.Collections.Generic;

namespace wasp.WebApi.Data.Entity
{
    public abstract class RecursiveEntity<TEntity, TType> : Entity<TType>, IRecursiveEntity<TEntity, TType> where TEntity : RecursiveEntity<TEntity, TType>
    {
        public virtual TEntity? Parent { get; set; }
        public virtual ICollection<TEntity> Children { get; set; } = new List<TEntity>();

        public virtual Dictionary<int, TEntity> GetParentObjectDictionary()
        {
            TEntity? parent = Parent;
            Dictionary<int, TEntity> entityDictionary = new ();
            int counter = 0;
            while (parent != null)
            {
                entityDictionary.Add(counter, parent);
                parent = parent.Parent;
                counter++;
            }
            return entityDictionary;
        }
    }
}