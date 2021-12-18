using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.FloatExtensions;

public class FloatBetween
{
    [Fact]
    public void Between()
    {
        const float b = 5;
        const float lower = 2;
        const float higher = 10;
            
        Assert.True(b.Between(lower, higher));
        Assert.False(higher.Between(lower, b));
        Assert.False(lower.Between(b, higher));
    }
}