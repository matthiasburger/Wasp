using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DecimalExtensions;

public class DecimalAbsolute
{
    [Fact]
    public void Absolute()
    {
        Assert.Equal(3m, (-3m).Absolute());
        Assert.Equal(3m, 3m.Absolute());
        Assert.Equal(0m, 0m.Absolute());
    }
}