using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions
{
    public class EnumerableDistinct
    {
        [Fact]
        public void Distinct()
        {
            IList<int>? items = 0.Range(50).ToList();

            Assert.Equal(items.Count, items.Distinct(x => x).Count());
            
            items.AddRange(0.Range(50));
            Assert.Equal(items.Count / 2, items.Distinct(x => x).Count());
        }
        [Fact]
        public void DistinctNullList()
        {
            IEnumerable<int>? items = null; 
            Assert.Throws<ArgumentNullException>(() => items!.Distinct(x => x).ToList());
        }
    }
}