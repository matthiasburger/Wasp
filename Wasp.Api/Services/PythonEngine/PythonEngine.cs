using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Python.Runtime;
using Autofac;

namespace Wasp.Api.Services.PythonEngine
{
    public class PythonNetEngine : IPythonEngine
    {
        private Lazy<PyModule> m_scope;
        
        // private PythonLogger m_logger = new PythonLogger();

        public PythonNetEngine()
        {
            m_scope = new Lazy<PyModule>(() => Py.CreateScope());
        }

        public void Dispose()
        {
            m_scope.Value.Dispose();
        }

        public string ExecuteCommand(string command)
        {

            string result = "OK";
            try
            {
                using (Py.GIL())
                {
                    var pyCompile = Python.Runtime.PythonEngine.Compile(command);
                    m_scope.Value.Execute(pyCompile);
                    // result = m_logger.ReadStream();
                    // m_logger.flush();

                    // Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(">"+command+"<");

                result = $"Trace: \n{ex.StackTrace} " + "\n" +
                    $"Message: \n {ex.Message}" + "\n";
                Console.WriteLine(result);
            }

            return result;
        }

        public void Initialize(IContainer appContainer)
        {
            SetVariable("DiContainer", new DiContainer(appContainer));
            var initScript = File.ReadAllText("./Python/HospitalApi/startup.py");
            ExecuteCommand(initScript);
        }

        public IList<string> SearchPaths()
        {
            var pythonPaths = new List<string>();
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

        public void SetSearchPath(IList<string> paths)
        {
            var searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            try { 
            using (Py.GIL())
            {
                string val = string.Join(", ", searchPaths.Select(x => $"'{x}'").ToArray());

                var src = "import sys\n" +
                           $"sys.path.extend({val})";
                ExecuteCommand(src);
            }
            }catch(Exception e)
            {

            }
        }

        public void SetVariable(string name, object value)
        {
            using (Py.GIL())
            {
                m_scope.Value.Set(name, value.ToPython());
            }
        }

        // private void SetupLogger()
        // {
        //     SetVariable("Logger", m_logger);
        //     const string loggerSrc = "import sys\n" +
        //                              "from io import StringIO\n" +
        //                              "sys.stdout = Logger\n" +
        //                              "sys.stdout.flush()\n" +
        //                              "sys.stderr = Logger\n" +
        //                              "sys.stderr.flush()\n";
        //     ExecuteCommand(loggerSrc);
        // }
    }

    class DiContainer
    {
        private readonly IContainer m_container;

        public DiContainer(IContainer container)
        {
            this.m_container = container;
        }
        public object Resolve(string typeFullName)
        {
            var type = GetType(typeFullName);
            return m_container.Resolve(type);
        }

        private Type GetType(string fullName)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            return types.Where(s => string.Equals(s.FullName, fullName, StringComparison.Ordinal)).FirstOrDefault();
        }
    }

    /*
    public class PythonLogger : IPythonLogger
    {
        private List<string> m_buffer;

        public PythonLogger()
        {
            m_buffer = new List<string>();
        }

        public void close()
        {
            m_buffer.Clear();
        }

        public void flush()
        {
            m_buffer.Clear();
        }

        public string ReadStream()
        {
            var str = string.Join("\n", m_buffer);
            return str;
        }

        public void write(string str)
        {
            if (str == "\n")
            {
                return;
            }

            m_buffer.Add(str);
            Trace.WriteLine(str);
        }

        public void writelines(string[] str)
        {
            m_buffer.AddRange(str);
        }
    }

    interface IPythonLogger
    {
        // flushes the buffer
        void flush();

        // writes str to the logger's buffer
        void write(string str);

        // writes multiple lines of strings to the logger's buffer
        void writelines(string[] str);

        // closes the buffer
        void close();

        // Reads the stream
        string ReadStream();
    }
    */
}
