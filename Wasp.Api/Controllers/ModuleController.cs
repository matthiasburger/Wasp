using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wasp.Api.Models;
using Wasp.Api.Services.PythonEngine;

namespace Wasp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly IPythonEngine m_pythonEngine;

        public ModuleController(ILogger<ModuleController> logger, IPythonEngine pythonEngine, IServiceProvider serviceProvider)
        
        {
            _logger = logger;
            m_pythonEngine = pythonEngine;

            m_pythonEngine.SetSearchPath(new List<string> { "./py/" });
        }

        [HttpGet("{id:long}")]
        public ActionResult Get(long id)
        {
            try
            {
                m_pythonEngine.ExecuteCommand("print('test')");

                return Ok(new { ok = true, called_at = DateTime.Now });
            }
            catch(Exception e)
            {

            }

            return Ok();
        }
    }
}