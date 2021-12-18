using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.LongExtensions;

public class LongBetween
{
    [Fact]
    public void Between()
    {
        const long b = 5;
        const long lower = 2;
        const long higher = 10;
            
        Assert.True(b.Between(lower, higher));
        Assert.False(higher.Between(lower, b));
        Assert.False(lower.Between(b, higher));
    }
}