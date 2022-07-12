using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wasp.WebApi.Services.PythonEngine;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PythonTestController : ControllerBase
    {
        public class ScriptContainer
        {
            public string Script { get; set; }
        }
        
        private readonly IPythonEngine _pythonEngine;

        public PythonTestController(IPythonEngine pythonEngine)
        {
            _pythonEngine = pythonEngine;
        }

        [HttpPost]
        public async Task<ActionResult> RunScript(ScriptContainer container)
        {
            string result = _pythonEngine.ExecuteCommand(container.Script);
            return Ok(result);
        }
    }
}