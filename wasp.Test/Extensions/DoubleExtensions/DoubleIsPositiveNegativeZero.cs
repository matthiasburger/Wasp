using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DoubleExtensions;

public class DoubleIsPositiveNegativeZero
{
    [Fact]
    public void IsPositiveNegativeZero()
    {
        for (double x = -10; x <= 10; x++)
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

                    double bZero = (double)x;
                    Assert.False(bZero.IsPositive());
                    Assert.True(bZero.IsZero());
                    break;
                    
                case > 0:
                    Assert.False(x.IsNegative());
                    Assert.True(x.IsPositive());
                    Assert.False(x.IsZero());
                        
                    double b = (double)x;
                    Assert.True(b.IsPositive());
                    Assert.False(b.IsZero());
                    break;
            }
        }
    }
}