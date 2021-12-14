using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ShortExtensions
{
    public class ShortAbsolute
    {
        [Fact]
        public void Absolute()
        {
            Assert.Equal((short)3, (short)(-3L).Absolute());
            Assert.Equal((short)3, (short)3.Absolute());
            Assert.Equal((short)0, (short)0.Absolute());
        }
    }
}