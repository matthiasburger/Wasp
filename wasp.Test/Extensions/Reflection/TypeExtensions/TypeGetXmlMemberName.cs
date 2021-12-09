using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions
{
    public class TypeGetXmlMemberName
    {
        [Fact]
        public void GetXmlMemberName()
        {
            Assert.Equal("System.String", typeof(string).GetXmlMemberName());
            Assert.Equal("System.Collections.Generic.IEnumerable`1", typeof(IEnumerable<string>).GetXmlMemberName());
            Assert.Equal("System.Collections.Generic.Dictionary`2", typeof(Dictionary<string, IEnumerable<string>>).GetXmlMemberName());
            Assert.Equal("System.ValueTuple`2", typeof((string name, int zahl)).GetXmlMemberName());
        }
    }
}