using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ShortExtensions
{
    public class ShortBetween
    {
        [Fact]
        public void Between()
        {
            const short b = 5;
            const short lower = 2;
            const short higher = 10;
            
            Assert.True(b.Between(lower, higher));
            Assert.False(higher.Between(lower, b));
            Assert.False(lower.Between(b, higher));
        }
    }
}