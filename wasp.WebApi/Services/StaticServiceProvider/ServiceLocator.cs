using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using wasp.WebApi.Exceptions;

namespace wasp.WebApi.Services.StaticServiceProvider
{
    public interface IServiceProviderProxy
    {
        T? GetService<T>();
        IEnumerable<T> GetServices<T>();
        object? GetService(Type type);
        IEnumerable<object?> GetServices(Type type);
    }
    
    public static class ServiceLocator
    {
        private static IServiceProviderProxy? _diProxy;

        public static IServiceProviderProxy ServiceProvider => _diProxy ?? throw new Exception("You should Initialize the ServiceProvider before using it.");

        public static void Initialize(IServiceProviderProxy proxy)
        {
            _diProxy = proxy;
        }
    }
    
    public class HttpContextServiceProviderProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor contextAccessor;

        public HttpContextServiceProviderProxy(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public T? GetService<T>()
        {
            if (contextAccessor.HttpContext is null)
                throw new CallDependencyInjectionException("contextAccessor.HttpContext is null");
            
            return contextAccessor.HttpContext.RequestServices.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            if (contextAccessor.HttpContext is null)
                throw new CallDependencyInjectionException("contextAccessor.HttpContext is null");
            
            return contextAccessor.HttpContext.RequestServices.GetServices<T>();
        }

        public object? GetService(Type type)
        {
            if (contextAccessor.HttpContext is null)
                throw new CallDependencyInjectionException("contextAccessor.HttpContext is null");
            
            return contextAccessor.HttpContext.RequestServices.GetService(type);
        }

        public IEnumerable<object?> GetServices(Type type)
        {
            if (contextAccessor.HttpContext is null)
                throw new CallDependencyInjectionException("contextAccessor.HttpContext is null");
            
            return contextAccessor.HttpContext.RequestServices.GetServices(type);
        }
    }
}