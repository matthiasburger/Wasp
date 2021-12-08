using System.Collections.Generic;
using System.Linq;

namespace wasp.WebApi.Data.Entity
{
    public interface IEntity<TType>
    {
        TType Id { get; set; }
    }
    
    public abstract class Entity<TType> : IEntity<TType>
    {
        public abstract TType Id { get; set; }
    }
    
    public interface IRecursiveEntity<TEntity, TType> where TEntity : IEntity<TType>
    {
        TEntity Parent { get; set; }
        ICollection<TEntity> Children { get; set; }
    }
    
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
    
    public static class RecursiveEntityEx
    {
        public static TEntity AddChild<TEntity, TType>(this TEntity parent, TEntity child) where TEntity : RecursiveEntity<TEntity, TType>
        {
            child.Parent = parent;
            parent.Children.Add(child);
            return parent;
        }

        public static TEntity AddChildren<TEntity, TType>(this TEntity parent, IEnumerable<TEntity> children) where TEntity : RecursiveEntity<TEntity, TType>
        {
            children.ToList().ForEach(c => parent.AddChild<TEntity, TType>(c));
            return parent;
        }
    }
}