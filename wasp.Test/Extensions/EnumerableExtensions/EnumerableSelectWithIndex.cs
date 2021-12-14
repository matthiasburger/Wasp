using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions
{
    public class EnumerableSelectWithIndex
    {
        [Theory]
        [InlineData(78, 56)]
        [InlineData(21, 78)]
        [InlineData(52, 74)]
        public void SelectWithIndex(int seed, int increment)
        {
            IEnumerable<int> values = seed.Range(increment);
            Assert.All(values.SelectWithIndex(), item => Assert.Equal(item.Index + seed, item.Item));
        }
    }
}