using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions;

public class TypeGetFullReadableName
{
    [Fact]
    public void GetFullReadableName()
    {
        Assert.Equal("System.String", typeof(string).GetFullReadableName());
        Assert.Equal("System.Collections.Generic.IEnumerable<System.String>", typeof(IEnumerable<string>).GetFullReadableName());
        Assert.Equal("System.Collections.Generic.Dictionary<System.String,System.Collections.Generic.IEnumerable<System.String>>", typeof(Dictionary<string, IEnumerable<string>>).GetFullReadableName());
        Assert.Equal("System.ValueTuple<System.String,System.Int32>", typeof((string name, int zahl)).GetFullReadableName());
    }
}