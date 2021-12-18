using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions;

public class TypeIsNullableType
{
    [Fact]
    public void IsNullableType()
    {
        Assert.False(typeof(string).IsNullableType());
        Assert.True(typeof(int?).IsNullableType());
    }
}