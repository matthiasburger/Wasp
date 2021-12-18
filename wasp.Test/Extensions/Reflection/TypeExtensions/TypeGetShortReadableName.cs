using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions;

public class TypeGetShortReadableName
{
    [Fact]
    public void GetShortReadableName()
    {
        Assert.Equal("String", typeof(string).GetShortReadableName());
        Assert.Equal("IEnumerable<String>", typeof(IEnumerable<string>).GetShortReadableName());
        Assert.Equal("Dictionary<String,IEnumerable<String>>", typeof(Dictionary<string, IEnumerable<string>>).GetShortReadableName());
        Assert.Equal("ValueTuple<String,Int32>", typeof((string name, int zahl)).GetShortReadableName());
    }
}