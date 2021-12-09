using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions
{
    public class StringCamelCase
    {
        [Theory]
        [InlineData("myProperty")]
        [InlineData("MyProperty")]
        [InlineData("My Property")]
        [InlineData("my-property")]
        [InlineData("my_property")]
        public void CamelCase(string value)
            => Assert.Equal("myProperty", value.CamelCase());

        [Theory]
        [InlineData("Myproperty")]
        public void CamelCaseOneWord(string value)
            => Assert.Equal("myproperty", value.CamelCase());
        
        [Theory]
        [InlineData(null)]
        public void CamelCaseNull(string value)
            => Assert.Throws<ArgumentNullException>(value.CamelCase);
        [Theory]
        [InlineData("")]
        public void CamelCaseEmpty(string value)
            => Assert.Equal(value, value.CamelCase()); 
    }
}