using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ByteExtensions
{
    public class ByteIsGreaterThanLowerThan
    {
        [Fact]
        public void IsGreaterThan()
        {
            const byte b = 5;
            
            Assert.True(b.IsGreaterThan(b-(b-1)));
            Assert.False(b.IsGreaterThan(b*2));
            Assert.False(b.IsGreaterThan(b));
        }
        
        [Fact]
        public void IsLowerThan()
        {
            const byte b = 5;
            
            Assert.False(b.IsLowerThan(b-(b-1)));
            Assert.True(b.IsLowerThan(b*2));
            Assert.False(b.IsLowerThan(b));
        }
    }
}