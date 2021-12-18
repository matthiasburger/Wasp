using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringPascalCase
{
    [Theory]
    [InlineData("myProperty")]
    [InlineData("MyProperty")]
    [InlineData("My Property")]
    [InlineData("my-property")]
    [InlineData("my_property")]
    public void PascalCase(string value)
        => Assert.Equal("MyProperty", value.PascalCase());

    [Theory]
    [InlineData(null)]
    public void PascalCaseNull(string value)
        => Assert.Throws<ArgumentNullException>(value.PascalCase);

    [Theory]
    [InlineData("")]
    public void PascalCaseEmpty(string value)
        => Assert.Equal(value, value.PascalCase());
}