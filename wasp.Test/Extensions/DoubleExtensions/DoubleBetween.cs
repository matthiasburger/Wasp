using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DoubleExtensions
{
    public class DoubleBetween
    {
        [Fact]
        public void Between()
        {
            const double b = 5;
            const double lower = 2;
            const double higher = 10;
            
            Assert.True(b.Between(lower, higher));
            Assert.False(higher.Between(lower, b));
            Assert.False(lower.Between(b, higher));
        }
    }
}