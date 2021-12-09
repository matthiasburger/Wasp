using System;
using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.GenericExtensions
{
    public class GenericIn
    {
        [Fact]
        public void InArray()
        {
            Assert.True(3.In(2, 3, 4));
            Assert.False(3.In(12, 13, 14));

            int[]? possibleValues = null;
            Assert.Throws<ArgumentNullException>(() => 3.In(possibleValues!));
            
            Assert.True('c'.In('a', 'c', 'e'));
        }
        
        [Fact]
        public void NotInArray()
        {
            Assert.False(3.NotIn(2, 3, 4));
            Assert.True(3.NotIn(12, 13, 14));

            int[]? possibleValues = null;
            Assert.Throws<ArgumentNullException>(() => 3.NotIn(possibleValues!));
            
            Assert.False('c'.NotIn('a', 'c', 'e'));
        }
        
        [Fact]
        public void InIEnumerable()
        {
            Assert.True(3.In(Enumerable.Range(1,3)));
            Assert.False(3.In(Enumerable.Range(10,3)));

            IEnumerable<int>? possibleValues = null;
            Assert.Throws<ArgumentNullException>(() => 3.In(possibleValues!));
        }
    }
}