using System;

using Microsoft.AspNetCore.Mvc;

using wasp.WebApi.Services.PythonEngine;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MigrationsController : ControllerBase
    {
        private readonly IPythonEngine _pythonEngine;

        public MigrationsController(IPythonEngine pythonEngine)
        {
            _pythonEngine = pythonEngine;
        }

        [HttpGet]
        public ActionResult Run(string migrationPackage)
        {
            string script = $@"
import inspect

from wasp.core.migration.migration_package import BaseMigrationPackage 
from wasp.app.migrations import {migrationPackage}

def get_classes_for_module(module, inheriting):
    for directory in dir(module):
        c = getattr(module, directory)
        
        if not inspect.isclass(c):
            continue

        s = c()
        if issubclass(c, inheriting):
            yield s

classes = get_classes_for_module({migrationPackage}, BaseMigrationPackage)
        
for clazz in classes:
    clazz.up()
";
            
            _pythonEngine.ExecuteCommand(script);
            return Ok(new { ok = true, called_at = DateTime.Now });
        }
    }
}
