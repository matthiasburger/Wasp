using System;
using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.ChangeTypeExtensions
{
    public class ChangeTypeTo
    {
        [Fact]
        public void To()
        {
            Assert.Equal(27.3, "27.3".To<double>());

            string? valueToConvert = null;
            Assert.Null(valueToConvert.To<double?>());
        }
        
        [Fact]
        public void ToOrDefault()
        {
            Assert.Equal(27.3, "27.3".ToOrDefault<double?>());

            string? valueToConvert = null;
            Assert.Equal(.0, valueToConvert.ToOrDefault<double>());
            Assert.Null(valueToConvert.ToOrDefault<double?>());
            Assert.Null("ab".ToOrDefault<double?>());
        }
        
        [Fact]
        public void ToOrNull()
        {
            Assert.Equal(27.3, "27.3".ToOrNull<double>());

            string? valueToConvert = null;
            Assert.Null(valueToConvert!.ToOrNull<double>());
            Assert.Null("ab".ToOrNull<double>());
        }
    }
}