using System;
using System.Reflection;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.MethodInfoExtensions
{
    public class MethodInfoIsExtensionMethod
    {
        [Fact]
        public void IsExtensionMethod()
        {
            MethodInfo? method = typeof(MethodInfoExtension)
                .GetMethod("IsExtensionMethod", BindingFlags.Public | BindingFlags.Static);
            
            Assert.NotNull(method);
            Assert.True(method!.IsExtensionMethod());
            
            MethodInfo? toStringMethod = typeof(string)
                .GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, Array.Empty<Type>());
            Assert.NotNull(toStringMethod);
            Assert.False(toStringMethod!.IsExtensionMethod());
        }
    }
}