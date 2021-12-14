using System.Collections.Generic;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.DictionaryExtensions
{
    public class DictionaryAddOrUpdate
    {
        [Fact]
        public void AddOrUpdate()
        {
            Dictionary<string, string> myDictionary = new ();
            Assert.DoesNotContain(myDictionary, x => x.Key == "key");

            myDictionary.AddOrUpdate("key", "value");
            Assert.Contains(myDictionary, x => x.Key == "key" && x.Value == "value");

            myDictionary.AddOrUpdate("key", "myValue");
            Assert.DoesNotContain(myDictionary, x => x.Key == "key" && x.Value == "value");
            Assert.Contains(myDictionary, x => x.Key == "key" && x.Value == "myValue");

            myDictionary.AddOrUpdate("another-key", "another-value");
            Assert.Contains(myDictionary, x => x.Key == "key" && x.Value == "myValue");
            Assert.Contains(myDictionary, x => x.Key == "another-key" && x.Value == "another-value");

        }
    }
}