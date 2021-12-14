using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DoubleExtensions
{
    public class DoubleMaxMin
    {
        [Fact]
        public void Max()
        {
            const double b = 5;
            
            Assert.Equal(double.MinValue, b.Max(double.MinValue));
            Assert.Equal(b, b.Max(double.MaxValue));
            Assert.Equal(b, b.Max(b));
        }
        
        [Fact]
        public void Min()
        {
            const double b = 5;
            
            Assert.Equal(b, b.Min(double.MinValue));
            Assert.Equal(double.MaxValue, b.Min(double.MaxValue));
            Assert.Equal(b, b.Min(b));
        }
    }
}