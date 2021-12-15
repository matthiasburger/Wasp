using System.Collections.Generic;
using System.Linq;

namespace wasp.WebApi.Data.Entity
{
    public static class RecursiveEntityExtension
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