using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.BooleanExtensions;

public class BooleanToInt
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToInt(bool value)
    {
        switch (value)
        {
            case true:
                Assert.Equal(1, value.ToInt());
                break;
            case false:
                Assert.Equal(0, value.ToInt());
                break;
        }
    }
}