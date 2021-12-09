using IronSphere.Extensions;
using Xunit;

namespace wasp.Test.Extensions.StringExtensions
{
    public class StringRemoveDiacritics
    {
        [Fact]
        public void RemoveDiacritics()
        {
            string? value = null;
            Assert.Null(value.RemoveDiacritics());
            Assert.Equal("aeiouaeiouaeioucaou", "áéíóúàèìòùâêîôûçäöü".RemoveDiacritics());
        }
    }
}