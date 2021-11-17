using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Python.Runtime;

namespace wasp.WebApi.Services.PythonEngine
{
    public class PythonNetEngine : IPythonEngine, IDisposable
    {
        private readonly Lazy<PyModule> _scope;

        public PythonNetEngine()
        {
            _scope = new Lazy<PyModule>(Py.CreateScope);
        }

        public void Dispose()
        {
            // _scope.Value.Dispose();
            GC.SuppressFinalize(this);
        }

        public string ExecuteCommand(string command)
        {
            string result = "OK";
            try
            {
                using (Py.GIL())
                {
                    PyObject pyCompile = Python.Runtime.PythonEngine.Compile(command);
                    _scope.Value.Execute(pyCompile);
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine(">" + command + "<");

                result = $"Trace: \n{ex.StackTrace} " + "\n" +
                         $"Message: \n {ex.Message}" + "\n";
                Console.WriteLine(result);
                #endif
            }

            return result;
        }

        public void Initialize(IDiContainer appContainer)
        {
            SetVariable("DiContainer", appContainer);
            string initScript = File.ReadAllText("./py/wasp/core/dependency_injection.py");
            ExecuteCommand(initScript);
        }

        public IList<string> SearchPaths()
        {
            List<string> pythonPaths = new();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                foreach (PyObject path in sys.path)
                {
                    string s = path.ToString();
                    pythonPaths.Add(s);
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
sys.path.append({string.Join(",", searchPaths.Select(x=>$"'{x}'").ToArray())})
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
    }
}