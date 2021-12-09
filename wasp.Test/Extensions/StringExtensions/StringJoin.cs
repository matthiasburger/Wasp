using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions
{
    public class StringJoin
    {
        [Fact]
        public void Join()
        {
            const string blindText = "The quick brown fox jumps over the lazy dog";
            string[] listOfStrings = blindText.Split();
            Assert.Equal(blindText, " ".Join(listOfStrings));
        }
        
        [Fact]
        public void JoinToNull()
        {
            const string blindText = "The quick brown fox jumps over the lazy dog";
            string[] listOfStrings = blindText.Split();
            Assert.Equal(blindText.Replace(" ", string.Empty), ((string?)null).Join(listOfStrings));
        }
        
        [Fact]
        public void JoinNullArray()
        {
            string[]? listOfStrings = null;
            Assert.Throws<ArgumentNullException>(() => ((string?)null).Join(listOfStrings!));
        }
        
        [Fact]
        public void JoinFunc()
        {
            const string blindText = "The quick brown fox jumps over the lazy dog";
            string[] listOfStrings = blindText.Split();
            Assert.Equal($" {blindText}", "".Join(listOfStrings, x=> $" {x}"));
        }
        
        [Fact]
        public void JoinFuncNullArray()
        {
            string[]? listOfStrings = null;
            Assert.Throws<ArgumentNullException>(() => ((string?)null).Join(listOfStrings!, x=>$"/{x}/"));
        }
    }
}