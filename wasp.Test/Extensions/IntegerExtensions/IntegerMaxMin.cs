using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.IntegerExtensions;

public class IntegerMaxMin
{
    [Fact]
    public void Max()
    {
        const int b = 5;
            
        Assert.Equal(int.MinValue, b.Max(int.MinValue));
        Assert.Equal(b, b.Max(int.MaxValue));
        Assert.Equal(b, b.Max(b));
    }
        
    [Fact]
    public void Min()
    {
        const int b = 5;
            
        Assert.Equal(b, b.Min(int.MinValue));
        Assert.Equal(int.MaxValue, b.Min(int.MaxValue));
        Assert.Equal(b, b.Min(b));
    }
}