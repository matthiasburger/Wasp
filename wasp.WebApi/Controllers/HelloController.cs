using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using wasp.WebApi.Services;
using wasp.WebApi.Services.PythonEngine;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly IPythonEngine _pythonEngine;
        private readonly IDiContainer _diContainer;

        public HelloController(IDiContainer diContainer, IPythonEngine pythonEngine)
        {
            _diContainer = diContainer;
            _pythonEngine = pythonEngine;
        }

        [HttpGet]
        public ActionResult Get()            
        {
            _pythonEngine.SetSearchPath(new List<string> { "./py/" });
            _pythonEngine.Initialize(_diContainer);
            _pythonEngine.ExecuteCommand(@"
from wasp.app.migrations.initial_migration import CreateBaseDatatables

CreateBaseDatatables().up()
");
            return Ok(new { ok = true, called_at = DateTime.Now });
        }
    }
}
