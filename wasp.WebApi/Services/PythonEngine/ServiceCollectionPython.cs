using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Python.Runtime;
using wasp.Core;
using wasp.WebApi.Services.Configuration;
using wasp.WebApi.Services.Environment;
using wasp.WebApi.Services.PythonLogger;

namespace wasp.WebApi.Services.PythonEngine
{
    using Environment = System.Environment;
    using PythonEngine = Python.Runtime.PythonEngine;
    using PythonLogger = wasp.WebApi.Services.PythonLogger.PythonLogger;

    public static class ServiceCollectionPython
    {

        public static IntPtr AddPython(this IServiceCollection serviceCollection, IConfiguration configuration, IEnvironmentDiscovery environment)
        {
            serviceCollection.AddTransient<IPythonLogger, PythonLogger>();
            serviceCollection.AddTransient<IPythonEngine, PythonNetEngine>();
            
            PythonSettings pythonSettings = configuration.GetSection(PythonSettings.SectionName).Get<PythonSettings>();

            Environment.SetEnvironmentVariable(@"PYTHONNET_PYDLL",pythonSettings.PythonNetPyDll);
            Runtime.PythonDLL = pythonSettings.PythonNetPyDll; 
            
            Environment.SetEnvironmentVariable(@"PATH", pythonSettings.Path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable(@"PYTHONHOME", pythonSettings.PythonHome, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable(@"PYTHONPATH", pythonSettings.PythonPath, EnvironmentVariableTarget.Process);
            
            PythonEngine.Initialize();
            return PythonEngine.BeginAllowThreads();
        }
    }
}