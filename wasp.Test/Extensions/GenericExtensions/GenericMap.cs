using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.GenericExtensions;

public class GenericMap
{
    private record Person(string? Name);

    [Fact]
    public void Map()
    {
        Person p = new("Markus");
        Assert.Equal(p.Name, p.Map(x => x.Name));
    }
}