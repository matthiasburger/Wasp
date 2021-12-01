using System;

using Microsoft.AspNetCore.Mvc;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ModuleController : ControllerBase
    {
        [HttpGet("{id:long}")]
        public ActionResult Open(long id)
        {
            
            
            return Ok(new { ok = true, called_at = DateTime.Now });
        }
    }
}