using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.LongExtensions;

public class LongIsPositiveNegativeZero
{
    [Fact]
    public void IsPositiveNegativeZero()
    {
        for (long x = -10; x <= 10; x++)
        {
            switch (x)
            {
                case < 0:
                    Assert.True(x.IsNegative());
                    Assert.False(x.IsPositive());
                    Assert.False(x.IsZero());
                    break;
                    
                case 0:
                    Assert.False(x.IsNegative());
                    Assert.False(x.IsPositive());
                    Assert.True(x.IsZero());

                    long bZero = (long)x;
                    Assert.False(bZero.IsPositive());
                    Assert.True(bZero.IsZero());
                    break;
                    
                case > 0:
                    Assert.False(x.IsNegative());
                    Assert.True(x.IsPositive());
                    Assert.False(x.IsZero());
                        
                    long b = (long)x;
                    Assert.True(b.IsPositive());
                    Assert.False(b.IsZero());
                    break;
            }
        }
    }
}