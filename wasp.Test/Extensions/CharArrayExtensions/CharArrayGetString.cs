using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.CharArrayExtensions;

public class CharArrayGetString
{
    [Fact]
    public void GetString()
    {
        char[]? charArray =
        {
            'H', 'e', 'l', 'l', 'o'
        };
        Assert.Equal("Hello", charArray.GetString());

        charArray = null;
        Assert.Throws<ArgumentNullException>(() => charArray.GetString());
    }
}