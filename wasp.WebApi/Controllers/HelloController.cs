using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Python.Runtime;

using wasp.WebApi.Services;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        Lazy<PyModule> m_scope;
        private readonly IDiContainer _diContainer;

        public HelloController(IDiContainer diContainer)
        {
            _diContainer = diContainer;
            m_scope = new Lazy<PyModule>(() => Py.CreateScope());
        }

        public class MyTemp
        {
               public string Name { get; set; }
        }


        public void SetSearchPath(IList<string> paths)
        {
            var searchPaths = paths.Where(Directory.Exists).Distinct().ToList();

            using (Py.GIL())
            {
                var src = $@"
import sys
sys.path.append({string.Join(",", searchPaths.Select(x=>$"'{x}'").ToArray())})

s = sys.path
";

                var pyCompile = PythonEngine.Compile(src);
                m_scope.Value.Execute(pyCompile);

                string sysPath = m_scope.Value.Get("s").ToString();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get()            
        {
            SetSearchPath(new List<string> { "./py/" });

            using (Py.GILState x = Py.GIL())
            {
                m_scope.Value.Set("DiContainer", _diContainer);
                var initScript = System.IO.File.ReadAllText("./py/wasp/core/dependency_injection.py");
                m_scope.Value.Execute(PythonEngine.Compile(initScript));


                var pyCompile = PythonEngine.Compile(@"
from wasp.app.migrations.initial_migration import CreateBaseDatatables

CreateBaseDatatables().up()
");
                m_scope.Value.Execute(pyCompile);


                return Ok(new { ok = true, called_at = DateTime.Now });

            }

        }
    }
}
