using System;
using System.Reflection;
using System.Text;
using IronSphere.Extensions.Reflection;
using Xunit;

namespace wasp.Test.Extensions.Reflection.ConstructorInfoExtensions
{
    public class ConstructorInfoGetXmlMemberName
    {
        [Fact]
        public void GetXmlMemberName()
        {
            ConstructorInfo? constructor = GetType()
                .GetConstructor(Array.Empty<Type>());
            Assert.NotNull(constructor);
            Assert.Equal("wasp.Test.Extensions.Reflection.ConstructorInfoExtensions.ConstructorInfoGetXmlMemberName.#ctor", constructor!.GetXmlMemberName());
            
            ConstructorInfo? nullConstructor = GetType()
                .GetConstructor(new []{typeof(string)});
            Assert.Null(nullConstructor!.GetXmlMemberName());
            
            ConstructorInfo? parameterConstructor = typeof(StringBuilder)
                .GetConstructor(new []{typeof(string)});
            Assert.Equal("System.Text.StringBuilder.#ctor(System.String)", parameterConstructor!.GetXmlMemberName());
        }
    }
}