using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DecimalExtensions
{
    public class DecimalMaxMin
    {
        [Fact]
        public void Max()
        {
            const decimal b = 5;
            
            Assert.Equal(decimal.MinValue, b.Max(decimal.MinValue));
            Assert.Equal(b, b.Max(decimal.MaxValue));
            Assert.Equal(b, b.Max(b));
        }
        
        [Fact]
        public void Min()
        {
            const decimal b = 5;
            
            Assert.Equal(b, b.Min(decimal.MinValue));
            Assert.Equal(decimal.MaxValue, b.Min(decimal.MaxValue));
            Assert.Equal(b, b.Min(b));
        }
    }
}