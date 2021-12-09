using System;

using IronSphere.Extensions;

using Xunit;

namespace wasp.Test.StringExtensions
{
    public class StringSubstring
    {
        [Theory]
        [InlineData("mein name ist matthias!")]
        public void Substring(string fullString)
        {
            Assert.Equal("name ist matthias", fullString.Substring("name", "!"));
        }

        [Fact]
        public void SubstringFailures()
        {
            const string testValue = "test";
            
            Assert.Throws<ArgumentException>(() => testValue.Substring("", "b")); // no start
            Assert.Throws<ArgumentException>(() => testValue.Substring("a", "")); // no end

            Assert.Null(testValue.Substring("a", "b")); // start doesn't exist
            Assert.Null(testValue.Substring("e", "b")); // end doesn't exist
        }
    }
}