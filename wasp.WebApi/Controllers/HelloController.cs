using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Python.Runtime;

using wasp.WebApi.Services;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly Lazy<PyModule> _pyScope;
        private readonly IDiContainer _diContainer;

        public HelloController(IDiContainer diContainer)
        {
            _diContainer = diContainer;
            _pyScope = new Lazy<PyModule>(Py.CreateScope);
        }

        public void SetSearchPath(IList<string> paths)
        {
            List<string> searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            using (Py.GIL())
            {
                string src = $@"
import sys
sys.path.append({string.Join(",", searchPaths.Select(x=>$"'{x}'").ToArray())})
";

                PyObject pyCompile = PythonEngine.Compile(src);
                _pyScope.Value.Execute(pyCompile);
            }
        }

        [HttpGet]
        public ActionResult Get()            
        {
            SetSearchPath(new List<string> { "./py/" });

            using (Py.GIL())
            {
                _pyScope.Value.Set("DiContainer", _diContainer);
                string initScript = System.IO.File.ReadAllText("./py/wasp/core/dependency_injection.py");
                _pyScope.Value.Execute(PythonEngine.Compile(initScript));

                PyObject pyCompile = PythonEngine.Compile(@"
from wasp.app.migrations.initial_migration import CreateBaseDatatables

CreateBaseDatatables().up()
");
                _pyScope.Value.Execute(pyCompile);


                return Ok(new { ok = true, called_at = DateTime.Now });

            }

        }
    }
}
