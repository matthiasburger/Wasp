using System;

using IronSphere.Extensions.Reflection;

namespace wasp.WebApi.Exceptions
{
    public class ServiceNotFoundException<T> : Exception
    {
        public ServiceNotFoundException():base($"could not find a service for {typeof(T).GetReadableName()}")
        {
            
        }
    }
}