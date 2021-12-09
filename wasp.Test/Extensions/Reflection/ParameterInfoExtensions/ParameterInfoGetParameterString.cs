using System.Collections.Generic;
using System.Reflection;
using System.Text;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.ParameterInfoExtensions
{
    public class ParameterInfoGetParameterString
    {
        [Fact]
        public void GetParameterString()
        {
            ParameterInfo[]? parameters = typeof(StringBuilder)
                .GetConstructor(new[] { typeof(string) })?
                .GetParameters();
            
            Assert.NotNull(parameters);
            Assert.Single(parameters!);

            ParameterInfo parameterInfo = parameters![0];
            Assert.Equal("System.String", parameterInfo.GetParameterString());
        }
        
        [Fact]
        public void GetParameterStringGeneric()
        {
            ParameterInfo[]? parameters = typeof(List<int>)
                .GetConstructor(new[] { typeof(IEnumerable<int>) })?
                .GetParameters();
            
            Assert.NotNull(parameters);
            Assert.Single(parameters!);

            ParameterInfo parameterInfo = parameters![0];
            Assert.Equal("System.Collections.Generic.IEnumerable{System.Int32}", parameterInfo.GetParameterString());
        }
        
        [Fact]
        public void GetParameterStringGenericMethod()
        {
            ParameterInfo[]? parameters = typeof(List<int>)
                .GetMethod("AddRange", BindingFlags.Instance | BindingFlags.Public)?
                .GetParameters();
            
            Assert.NotNull(parameters);
            Assert.Single(parameters!);

            ParameterInfo parameterInfo = parameters![0];
            Assert.Equal("System.Collections.Generic.IEnumerable{System.Int32}", parameterInfo.GetParameterString());
        }
    }
}