using System;
using IronSphere.Extensions.Reflection;

namespace wasp.WebApi.Exceptions
{
    public class MissingParameterException<T> : Exception
    {
        public MissingParameterException(string parameterName, string property):base(_getMessage(parameterName, property))
        {
        }

        public MissingParameterException(string parameterName, string property, Exception innerException):base(_getMessage(parameterName, property), innerException)
        {
        }
        
        private static string _getMessage(string parameterName, string property)
        {
            return $"{parameterName}[{typeof(T).GetShortReadableName()}] -> {property}";
        }
    }
}