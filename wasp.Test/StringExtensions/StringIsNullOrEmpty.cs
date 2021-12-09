using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.StringExtensions
{
    public class StringIsNullOrEmpty
    {
        [Theory]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("   test   ")]
        public void NullOrEmpty(string data)
            => Assert.Equal(string.IsNullOrEmpty(data), data.IsNullOrEmpty());
        
        [Theory]
        [InlineData("    ", "<none>")]
        [InlineData("", "<none>")]
        [InlineData(null, "<none>")]
        [InlineData("test", "<none>")]
        [InlineData("   test   ", "<none>")]
        public void ValueNullOrEmpty(string data, string defaultValue)
        {
            Assert.Equal(string.IsNullOrEmpty(data) ? defaultValue : data, data.ValueIfNullOrEmpty(defaultValue));
        }
    }
}