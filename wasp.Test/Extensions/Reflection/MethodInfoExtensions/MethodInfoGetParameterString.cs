using System.Collections.Generic;
using System.Reflection;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.MethodInfoExtensions
{
    public class MethodInfoGetParameterString
    {
        [Fact]
        public void GetXmlMemberName()
        {
            MethodInfo? methodInfo = typeof(List<int>)
                .GetMethod("AddRange", BindingFlags.Instance | BindingFlags.Public);
            
            Assert.NotNull(methodInfo);
            Assert.Equal("System.Collections.Generic.List`1.AddRange(System.Collections.Generic.IEnumerable{System.Int32})",
                methodInfo!.GetXmlMemberName());
        }
    }
}