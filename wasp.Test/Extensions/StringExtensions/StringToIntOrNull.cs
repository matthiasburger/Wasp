using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions;

public class StringToIntOrNull
{
    [Fact]
    public void ToIntOrNull()
    {
        Assert.Equal(3, "3".ToIntOrNull());
        Assert.Null("".ToIntOrNull());
        string? nullString = null;
        Assert.Null(nullString.ToIntOrNull());
    }
}