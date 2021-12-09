using System;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.StringExtensions
{
    public class StringStartsEnsContainsAny
    {
        [Fact]
        public void StartsWithAny()
        {
            Assert.True("test".StartsWithAny("x", "tx", "tes"));
            Assert.True("test".StartsWithAny("x", "tx", "test"));
            Assert.False("test".StartsWithAny("x", "tx", "st"));
            Assert.False("".StartsWithAny("x", "tx", "st"));
            Assert.Throws<ArgumentNullException>(() => ((string?)null).StartsWithAny("x", "tx", "st"));
            Assert.Throws<ArgumentNullException>(() => "test".StartsWithAny(null));
            Assert.False("test".StartsWithAny());
        }
        
        [Fact]
        public void EndsWithAny()
        {
            Assert.True("test".EndsWithAny("x", "tx", "est"));
            Assert.True("test".EndsWithAny("x", "tx", "test"));
            Assert.False("test".EndsWithAny("x", "tx", "tes"));
            Assert.False("".EndsWithAny("x", "tx", "st"));
            Assert.Throws<ArgumentNullException>(() => ((string?)null).EndsWithAny("x", "tx", "st"));
            Assert.Throws<ArgumentNullException>(() => "test".EndsWithAny(null));
            Assert.False("test".EndsWithAny());
        }
        
        [Fact]
        public void ContainsAny()
        {
            Assert.True("test".ContainsAny("x", "tx", "est"));
            Assert.True("test".ContainsAny("x", "tx", "test"));
            Assert.True("test".ContainsAny("x", "tx", "tes"));
            Assert.True("test".ContainsAny("x", "tx", "es"));
            Assert.False("".ContainsAny("x", "tx", "st"));
            Assert.Throws<ArgumentNullException>(() => ((string?)null).ContainsAny("x", "tx", "st"));
            Assert.Throws<ArgumentNullException>(() => "test".ContainsAny(null));
            Assert.False("test".ContainsAny());
        }
    }
}