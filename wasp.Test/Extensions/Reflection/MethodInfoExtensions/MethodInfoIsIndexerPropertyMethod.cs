using System;
using System.Collections.Generic;
using System.Reflection;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.MethodInfoExtensions
{
    public class MethodInfoIsIndexerPropertyMethod
    {
        [Fact]
        public void IsIndexerPropertyMethod()
        {
            MethodInfo toStringMethod = typeof(string)
                .GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, Array.Empty<Type>())!;
            Assert.NotNull(toStringMethod);
            Assert.False(toStringMethod.IsIndexerPropertyMethod());

            Dictionary<string, string> dictionary = new();
            MethodInfo setMethod = dictionary.GetType().GetMethod("set_Item")!;
            Assert.True(setMethod.IsIndexerPropertyMethod());
            MethodInfo getMethod = dictionary.GetType().GetMethod("get_Item")!;
            Assert.True(getMethod.IsIndexerPropertyMethod());
        }
    }
}