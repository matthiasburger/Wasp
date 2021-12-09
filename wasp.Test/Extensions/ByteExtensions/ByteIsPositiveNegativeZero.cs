using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ByteExtensions
{
    public class ByteIsPositiveNegativeZero
    {
        [Fact]
        public void IsPositiveNegativeZero()
        {
            for (sbyte x = -10; x <= 10; x++)
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

                        byte bZero = (byte)x;
                        Assert.False(bZero.IsPositive());
                        Assert.True(bZero.IsZero());
                        break;
                    
                    case > 0:
                        Assert.False(x.IsNegative());
                        Assert.True(x.IsPositive());
                        Assert.False(x.IsZero());
                        
                        byte b = (byte)x;
                        Assert.True(b.IsPositive());
                        Assert.False(b.IsZero());
                        break;
                }
            }
        }
    }
}