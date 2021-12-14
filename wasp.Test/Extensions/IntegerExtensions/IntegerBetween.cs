using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.IntegerExtensions
{
    public class IntegerBetween
    {
        [Fact]
        public void Between()
        {
            const int b = 5;
            const int lower = 2;
            const int higher = 10;
            
            Assert.True(b.Between(lower, higher));
            Assert.False(higher.Between(lower, b));
            Assert.False(lower.Between(b, higher));
        }
    }
}