using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace wasp.Core.Extensions
{
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "TODO: move this to IronSphere.Extensions")]
    public static class EnumerableExtensions
    {
        [Obsolete("Use IronSphere.Extensions > 21.11.0")]
        private class IndexedItem<TItem> : IIndexedItem<TItem>
        {
            public IndexedItem(TItem item, int index)
            {
                Item = item;
                Index = index;
            }

            public TItem Item { get; }

            public int Index { get; }
        }
        
        [Obsolete("Use IronSphere.Extensions > 21.11.0")]
        public static IEnumerable<IIndexedItem<TModel>> SelectWithIndex<TModel>(this IEnumerable<TModel> collection)
        {
            return collection.Select((item, x) => new IndexedItem<TModel>(item, x));
        }
    }
    
    [Obsolete("Use IronSphere.Extensions > 21.11.0")]
    public interface IIndexedItem<out TItem>
    {
        TItem Item { get; }
        int Index { get; }
    }
}