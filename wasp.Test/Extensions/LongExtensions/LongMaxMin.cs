using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.LongExtensions;

public class LongMaxMin
{
    [Fact]
    public void Max()
    {
        const long b = 5;
            
        Assert.Equal(long.MinValue, b.Max(long.MinValue));
        Assert.Equal(b, b.Max(long.MaxValue));
        Assert.Equal(b, b.Max(b));
    }
        
    [Fact]
    public void Min()
    {
        const long b = 5;
            
        Assert.Equal(b, b.Min(long.MinValue));
        Assert.Equal(long.MaxValue, b.Min(long.MaxValue));
        Assert.Equal(b, b.Min(b));
    }
}