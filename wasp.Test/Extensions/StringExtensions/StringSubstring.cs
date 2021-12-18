using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringSubstring
{
    [Theory]
    [InlineData("mein name ist matthias!")]
    public void Substring(string fullString)
    {
        Assert.Equal("name ist matthias", fullString.Substring("name", "!"));
        Assert.Equal("name ist matthias", fullString.SubString("name", "!"));
        Assert.Equal("name ist matthias!", fullString.Substring("name", null));
    }

    [Fact]
    public void SubstringFailures()
    {
        const string testValue = "test";
            
        Assert.Throws<ArgumentException>(() => testValue.Substring("", "b")); // no start

        Assert.Null(testValue.Substring("a", "b")); // start doesn't exist
        Assert.Null(testValue.Substring("e", "b")); // end doesn't exist
            
            
            
        Assert.Throws<ArgumentException>(() => testValue.SubString("", "b")); // no start
        Assert.Throws<ArgumentException>(() => testValue.SubString("a", "")); // no end

        Assert.Null(testValue.SubString("a", "b")); // start doesn't exist
        Assert.Null(testValue.SubString("e", "b")); // end doesn't exist

    }
}