using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Python.Runtime;

using wasp.WebApi.Services.Configuration;

namespace wasp.WebApi.Services.PythonEngine
{
    using Environment = System.Environment;
    using PythonEngine = Python.Runtime.PythonEngine;
    
    public static class ServiceCollectionPython
    {
        public static IntPtr AddPython(this IServiceCollection _, IConfiguration configuration)
        {
            PythonSettings pythonSettings = configuration.GetSection(PythonSettings.SectionName).Get<PythonSettings>();
            Environment.SetEnvironmentVariable(@"PYTHONNET_PYDLL",pythonSettings.PythonDll);
            Runtime.PythonDLL = pythonSettings.PythonDll;
            
            Environment.SetEnvironmentVariable(@"PATH", pythonSettings.Path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable(@"PYTHONHOME", pythonSettings.Path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable(@"PYTHONPATH", $"{pythonSettings.Path}\\Lib\\site-packages;{pythonSettings.Path}\\Lib", EnvironmentVariableTarget.Process);

            PythonEngine.Initialize();
            return PythonEngine.BeginAllowThreads();
        }
    }
}