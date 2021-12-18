using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ByteExtensions;

public class ByteMaxMin
{
    [Fact]
    public void Max()
    {
        const byte b = 5;
            
        Assert.Equal(byte.MinValue, b.Max(byte.MinValue));
        Assert.Equal(b, b.Max(byte.MaxValue));
        Assert.Equal(b, b.Max(b));
    }
        
    [Fact]
    public void Min()
    {
        const byte b = 5;
            
        Assert.Equal(b, b.Min(byte.MinValue));
        Assert.Equal(byte.MaxValue, b.Min(byte.MaxValue));
        Assert.Equal(b, b.Min(b));
    }
}