using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.FloatExtensions;

public class FloatMaxMin
{
    [Fact]
    public void Max()
    {
        const float b = 5;
            
        Assert.Equal(float.MinValue, b.Max(float.MinValue));
        Assert.Equal(b, b.Max(float.MaxValue));
        Assert.Equal(b, b.Max(b));
    }
        
    [Fact]
    public void Min()
    {
        const float b = 5;
            
        Assert.Equal(b, b.Min(float.MinValue));
        Assert.Equal(float.MaxValue, b.Min(float.MaxValue));
        Assert.Equal(b, b.Min(b));
    }
}