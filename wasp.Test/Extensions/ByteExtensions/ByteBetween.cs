using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ByteExtensions
{
    public class ByteBetween
    {
        [Fact]
        public void Between()
        {
            const byte b = 5;
            const byte lower = 2;
            const byte higher = 10;
            
            Assert.True(b.Between(lower, higher));
            Assert.False(higher.Between(lower, b));
            Assert.False(lower.Between(b, higher));
        }
        
        [Fact]
        public void BetweenOrEquals()
        {
            const byte b = 5;
            const byte lower = 2;
            const byte higher = 10;
            
            Assert.True(b.BetweenOrEquals(lower, higher));
            Assert.True(b.BetweenOrEquals(b, b));
            Assert.True(b.BetweenOrEquals(b, higher));
            Assert.True(b.BetweenOrEquals(lower, b));
            Assert.False(higher.BetweenOrEquals(lower, b));
            Assert.False(lower.BetweenOrEquals(b, higher));
        }
    }
}