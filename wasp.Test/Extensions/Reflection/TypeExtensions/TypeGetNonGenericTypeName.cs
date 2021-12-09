using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions
{
    public class TypeGetNonGenericTypeName
    {
        [Fact]
        public void GetNonGenericTypeName()
        {
            Assert.Equal("System.String", typeof(string).GetNonGenericTypeName());
            Assert.Equal("System.Collections.Generic.IEnumerable", typeof(IEnumerable<string>).GetNonGenericTypeName());
            Assert.Equal("System.Collections.Generic.Dictionary", typeof(Dictionary<string, IEnumerable<string>>).GetNonGenericTypeName());
            Assert.Equal("System.ValueTuple", typeof((string name, int zahl)).GetNonGenericTypeName());
        }
    }
}