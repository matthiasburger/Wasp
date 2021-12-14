using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DecimalExtensions
{
    public class DecimalBetween
    {
        [Fact]
        public void Between()
        {
            const decimal b = 5;
            const decimal lower = 2;
            const decimal higher = 10;
            
            Assert.True(b.Between(lower, higher));
            Assert.False(higher.Between(lower, b));
            Assert.False(lower.Between(b, higher));
        }
    }
}