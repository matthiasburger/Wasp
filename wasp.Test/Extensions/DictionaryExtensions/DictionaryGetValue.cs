using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DictionaryExtensions;

public class DictionaryGetValue
{
    [Fact]
    public void TestGetValue()
    {
        Dictionary<string, int>? myDictionary = new()
        {
            { "key", 1 }
        };

        Assert.Equal(1, myDictionary.GetValue("key"));
        Assert.Equal(0, myDictionary.GetValue("key2"));

        myDictionary = null;
        Assert.Throws<ArgumentNullException>(() => myDictionary!.GetValue("key"));
    }
        
    [Fact]
    public void TestGetValueKvPair()
    {
        Dictionary<string, int> myDictionary = new()
        {
            { "key", 1 }
        };

        Assert.Equal(1, myDictionary.ToList().GetValue("key"));
        Assert.Equal(0, myDictionary.ToList().GetValue("key2"));

        IEnumerable<KeyValuePair<string, int>>? myKvPairList = null;
        Assert.Throws<ArgumentNullException>(() => myKvPairList!.GetValue("key"));
    }

    [Fact]
    public void TestGetValueNameValueCollection()
    {
        NameValueCollection? myDictionary = new()
        {
            { "key", "1" },
            { "key3", "<not convertable>" }
        };

        Assert.Equal("1", myDictionary.GetValue<string>("key"));
        Assert.Equal(1, myDictionary.GetValue<int>("key"));
        Assert.Throws<FormatException>(() => myDictionary.GetValue<int?>("key3"));
        Assert.Null(myDictionary.GetValue<string>("key2"));

        myDictionary = null;
        Assert.Throws<ArgumentNullException>(() => myDictionary!.GetValue<string>("key"));
    }
}