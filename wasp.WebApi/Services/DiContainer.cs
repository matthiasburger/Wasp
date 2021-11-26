using System;
using System.Collections.Generic;
using System.Linq;

namespace wasp.WebApi.Services
{
    public class DiContainer : IDiContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public DiContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Resolve(string name)
        {
            Type type = GetType(name);
            return _serviceProvider.GetService(type);

        }

        private static Type GetType(string fullName)
        {
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            return types.FirstOrDefault(s => string.Equals(s.FullName, fullName, StringComparison.Ordinal));
        }
    }
}
