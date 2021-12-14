using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ShortExtensions
{
    public class ShortMaxMin
    {
        [Fact]
        public void Max()
        {
            const short b = 5;
            
            Assert.Equal(short.MinValue, b.Max(short.MinValue));
            Assert.Equal(b, b.Max(short.MaxValue));
            Assert.Equal(b, b.Max(b));
        }
        
        [Fact]
        public void Min()
        {
            const short b = 5;
            
            Assert.Equal(b, b.Min(short.MinValue));
            Assert.Equal(short.MaxValue, b.Min(short.MaxValue));
            Assert.Equal(b, b.Min(b));
        }
    }
}