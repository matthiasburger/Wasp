using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DoubleExtensions
{
    public class DoubleAbsolute
    {
        [Fact]
        public void Absolute()
        {
            Assert.Equal(3.0, (-3.0).Absolute());
            Assert.Equal(3.0, 3.0.Absolute());
            Assert.Equal(.0, .0.Absolute());
        }
    }
}