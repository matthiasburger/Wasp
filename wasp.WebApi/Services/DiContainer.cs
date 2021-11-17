using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Python.Runtime;

namespace wasp.WebApi.Services
{
    public interface IDiContainer
    {
        object Resolve(string name);
    }

    public class DiContainer : IDiContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public DiContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Resolve(string name)
        {
            var type = GetType(name);
            return _serviceProvider.GetService(type);

        }

        private Type GetType(string fullName)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            return types.Where(s => string.Equals(s.FullName, fullName, StringComparison.Ordinal)).FirstOrDefault();
        }
    }
}
