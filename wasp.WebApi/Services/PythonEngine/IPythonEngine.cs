using System.Collections.Generic;

namespace wasp.WebApi.Services.PythonEngine
{
    public interface IPythonEngine
    {
        string ExecuteCommand(string command);

        void SetVariable(string name, object value);

        IList<string> SearchPaths();

        void SetSearchPath(IEnumerable<string> paths);
    }
}