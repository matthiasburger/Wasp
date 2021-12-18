using System;
using System.Collections.Generic;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.AnonymousExtensions;

public class AnonymousObjectToDictionary
{
    [Fact]
    public void ToDictionary()
    {
        var anonymous = new { name="Liselotte" };

        IDictionary<string, string> dictionary = anonymous.ToDictionary<string>();
        Assert.True(dictionary.ContainsKey("name"));
        Assert.Equal(dictionary["name"], anonymous.name);

        object? anonymousNull = null;

        Assert.Throws<ArgumentNullException>(() => anonymousNull.ToDictionary<string>());
        Assert.Throws<InvalidCastException>(() => dictionary.ToDictionary<double>());
    }
}