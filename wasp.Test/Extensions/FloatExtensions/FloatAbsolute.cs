using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.FloatExtensions;

public class FloatAbsolute
{
    [Fact]
    public void Absolute()
    {
        Assert.Equal(3f, (-3f).Absolute());
        Assert.Equal(3f, 3f.Absolute());
        Assert.Equal(0f, 0f.Absolute());
    }
}