using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Python.Runtime;

using wasp.WebApi.Services.PythonLogger;

namespace wasp.WebApi.Services.PythonEngine
{
    public class PythonNetEngine : IPythonEngine, IDisposable
    {
        private readonly Lazy<PyModule> _scope;
        private readonly IDiContainer _diContainer;
        private readonly IPythonLogger _pythonLogger;

        public PythonNetEngine(IDiContainer diContainer, IPythonLogger pythonLogger)
        {
            _scope = new Lazy<PyModule>(Py.CreateScope);

            _pythonLogger = pythonLogger;
            _setupLogger();

            SetSearchPath(new List<string> { "./py/" });
            _diContainer = diContainer;
            _initialize();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("ReSharper", "RedundantCatchClause", Justification = "catch clause is in DEBUG different")]
        public string ExecuteCommand(string command)
        {
            string result;
            try
            {
                using (Py.GIL())
                {
                    PyObject pyCompile = Python.Runtime.PythonEngine.Compile(command);
                    _scope.Value.Execute(pyCompile);
                    
                    result = _pythonLogger.ReadStream();
                    _pythonLogger.flush();
                }
            }
            #if DEBUG
            catch (PythonException)
            {
                throw;
            }
            #else
            catch (PythonException ex)
            {
                result = ex.ToString();
                Console.Error.WriteLine(ex.ToString());
            }
            #endif

            return result;
        }

        public void Initialize(IDiContainer appContainer)
        {
            SetVariable("DiContainer", appContainer);
            string initScript = File.ReadAllText("./py/wasp/core/dependency_injection.py");
            ExecuteCommand(initScript);
        }

        private void _initialize() => Initialize(_diContainer);

        public IList<string> SearchPaths()
        {
            List<string> pythonPaths = new();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                foreach (PyObject path in sys.path)
                {
                    pythonPaths.Add(path.ToString());
                }
            }

            return pythonPaths.Select(Path.GetFullPath).ToList();
        }

        public void SetSearchPath(IEnumerable<string> paths)
        {
            List<string> searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            using (Py.GIL())
            {
                string src = $@"
import sys
sys.path.append({string.Join(",", searchPaths.Select(x => $"'{x}'").ToArray())})
";

                PyObject pyCompile = Python.Runtime.PythonEngine.Compile(src);
                _scope.Value.Execute(pyCompile);
            }
        }

        public void SetVariable(string name, object value)
        {
            using (Py.GIL())
            {
                _scope.Value.Set(name, value.ToPython());
            }
        }

        private void _setupLogger()
        {
            SetVariable("Logger", (PythonLogger.PythonLogger)_pythonLogger);
            const string loggerSrc = @"
import sys
from io import StringIO
sys.stdout = Logger
sys.stdout.flush()
sys.stderr = Logger
sys.stderr.flush()
";
            ExecuteCommand(loggerSrc);
        }
    }
}