using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.IntegerExtensions
{
    public class IntegerAbsolute
    {
        [Fact]
        public void Absolute()
        {
            Assert.Equal(3, (-3).Absolute());
            Assert.Equal(3, 3.Absolute());
            Assert.Equal(0, 0.Absolute());
        }
    }
}