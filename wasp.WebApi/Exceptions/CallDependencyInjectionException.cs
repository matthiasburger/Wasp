using System;

namespace wasp.WebApi.Exceptions
{
    public class CallDependencyInjectionException : Exception
    {
        public CallDependencyInjectionException(string message): base(message)
        {
        }
        
        public CallDependencyInjectionException(string message, Exception inner): base(message, inner)
        {
        }
    }
}