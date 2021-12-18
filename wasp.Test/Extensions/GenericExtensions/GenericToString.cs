using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.GenericExtensions;

public class GenericToString
{
    private record Person(string? Name);

    [Fact]
    public void ToStringTest()
    {
        Person p = new("Markus");
        Assert.Equal($"Hello {p.Name}", p.ToString(x => $"Hello {x.Name}"));

        Func<Person, string>? expression = null;
        Assert.Throws<ArgumentNullException>(() => p.ToString(expression!));
    }
}