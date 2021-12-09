using System;
using System.Collections.Generic;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.TypeExtensions
{
    public class TypeIsAnonymousType
    {
        [Fact]
        public void IsAnonymousType()
        {
            object anonymousType = new { a = 1 };
            Dictionary<string, object> dictionary = new ();

            Type? nullType = null;
            
            Assert.True(anonymousType.GetType().IsAnonymousType());
            Assert.False(dictionary.GetType().IsAnonymousType());
            Assert.Throws<ArgumentNullException>(()=>nullType!.IsAnonymousType());
        }
    }
}