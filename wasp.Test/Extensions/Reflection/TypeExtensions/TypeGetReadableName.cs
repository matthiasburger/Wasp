using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions;

public class TypeGetReadableName
{
    [Fact]
    public void GetShortReadableName()
    {
        Assert.Equal("String", typeof(string).GetReadableName());
        Assert.Equal("IEnumerable<String>", typeof(IEnumerable<string>).GetReadableName());
        Assert.Equal("Dictionary<String,IEnumerable<String>>", typeof(Dictionary<string, IEnumerable<string>>).GetReadableName());
        Assert.Equal("ValueTuple<String,Int32>", typeof((string name, int zahl)).GetReadableName());
    }
}