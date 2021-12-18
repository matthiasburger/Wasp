using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringLike
{
    [Fact]
    public void Like()
    {
        Assert.True("bla".Like("*l*"));
        Assert.True("02D4BN".Like("[0-9A-Z]{6}"));
        Assert.Throws<ArgumentException>(() => "02D4BN".Like("["));

        string? nullPattern = null;
        Assert.False("02D4BN".Like(nullPattern));
            
        string emptyPattern = string.Empty;
        Assert.False("02D4BN".Like(emptyPattern));

        string? nullValue = null;
        Assert.False(nullValue.Like("/d"));
    }
}