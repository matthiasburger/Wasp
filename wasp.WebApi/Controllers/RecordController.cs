using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using wasp.WebApi.Controllers.Base;
using wasp.WebApi.Data;
using wasp.WebApi.Data.Dto;
using wasp.WebApi.Data.Mts;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RecordController : ApiBaseController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public RecordController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task Save(MtsRecord record)
        {
            
        }
    }
}