using System.Collections.Generic;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DictionaryExtensions;

public class DictionaryGetOrCreate
{
    [Fact]
    public void GetOrCreate()
    {
        Dictionary<string, string> myDictionary = new();
        Assert.DoesNotContain(myDictionary, x=>x.Key == "foo");

        string val = myDictionary.GetOrCreate("foo", _ => "bar");
        Assert.Contains(myDictionary, x=>x.Key == "foo" && x.Value == "bar");
        Assert.Equal("bar", val);

        string existingValue = myDictionary.GetOrCreate("foo", x => x);
        Assert.Contains(myDictionary, x=>x.Key == "foo" && x.Value == "bar");
        Assert.DoesNotContain(myDictionary, x=>x.Key == "foo" && x.Value == "foo");
        Assert.Equal("bar", existingValue);
    }
}