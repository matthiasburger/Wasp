using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions
{
    public class StringIsNullOrWhiteSpace
    {
        [Theory]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("   test   ")]
        public void NullOrEmpty(string data)
            => Assert.Equal(string.IsNullOrWhiteSpace(data), data.IsNullOrWhiteSpace());
        
        [Theory]
        [InlineData("    ", "<none>")]
        [InlineData("", "<none>")]
        [InlineData(null, "<none>")]
        [InlineData("test", "<none>")]
        [InlineData("   test   ", "<none>")]
        public void ValueNullOrWhiteSpace(string data, string defaultValue)
        {
            Assert.Equal(string.IsNullOrWhiteSpace(data) ? defaultValue : data, data.ValueIfNullOrWhiteSpace(defaultValue));
        }
    }
}