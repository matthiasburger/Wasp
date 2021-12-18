using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions;

public class TypeGetNullableUnderlyingType
{
    [Fact]
    public void GetNullableUnderlyingType()
    {
        Assert.Null(typeof(string).GetNullableUnderlyingType());
        Assert.Null(typeof(List<string>).GetNullableUnderlyingType());
        Assert.Null(typeof(string[]).GetNullableUnderlyingType());
        Assert.Equal(typeof(int), typeof(int?).GetNullableUnderlyingType());
    }
}