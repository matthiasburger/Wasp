using System.Collections.Generic;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.EnumerableExtensions
{
    public class EnumerableShuffle
    {
        [Fact]
        public void Shuffle()
        {
            IEnumerable<int> items = 0.Range(50).ToList();

            Assert.True(items.Loops(x => x + 1));

            items = items.Shuffle();
            
            Assert.False(items.Loops(x => x + 1));
        }
        [Fact]
        public void Randomize()
        {
            IEnumerable<int> items = 0.Range(50).ToList();

            Assert.True(items.Loops(x => x + 1));

            items = items.Randomize();
            
            Assert.False(items.Loops(x => x + 1));
        }
    }
}