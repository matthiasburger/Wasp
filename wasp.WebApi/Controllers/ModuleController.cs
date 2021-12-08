using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using wasp.WebApi.Data;
using wasp.WebApi.Data.Models;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        
        public ModuleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> Open(string id)
        {
            Module module = await _dbContext.Modules
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataFields)
                .ThenInclude(x => x.DataItem)
                .SingleAsync(x=>x.Id == id);
            
            return Ok(new { ok = true, called_at = DateTime.Now });
        }
        
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            Module module = new ()
            {
                Name = "Test-Module"
            };

            await _dbContext.AddAsync(module);
            await _dbContext.SaveChangesAsync();
            
            return Ok(new { ok = true, called_at = DateTime.Now });
        }
    }
}