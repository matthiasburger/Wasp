using System;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.StringExtensions
{
    public class StringCapitalize
    {
        [Theory]
        [InlineData("myProperty")]
        [InlineData("MyProperty")]
        public void Capitalize(string value)
            => Assert.Equal("MyProperty", value.Capitalize());
        
        [Fact]
        public void CapitalizeNull()
            => Assert.Throws<ArgumentNullException>(((string?)null).Capitalize);
        [Fact]
        public void CapitalizeEmpty()
            => Assert.Equal(string.Empty, string.Empty.Capitalize());
    }
}