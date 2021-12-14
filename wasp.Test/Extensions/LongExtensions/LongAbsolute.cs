using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.LongExtensions
{
    public class LongAbsolute
    {
        [Fact]
        public void Absolute()
        {
            Assert.Equal(3L, (-3L).Absolute());
            Assert.Equal(3L, 3L.Absolute());
            Assert.Equal(0L, 0L.Absolute());
        }
    }
}