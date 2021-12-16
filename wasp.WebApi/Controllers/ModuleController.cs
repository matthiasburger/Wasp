using System;
using System.Collections.Generic;
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
        
        [HttpGet("demo")]
        public async Task<ActionResult> OpenDemo()
        {
            #region demodata
            DataTable project = new()
            {
                SqlId = "Project"
            };
            DataTable task = new()
            {
                SqlId = "Task"
            };
            DataTable resource = new()
            {
                SqlId = "Resource"
            };

            DataItem projectId = new()
            {
                DataTable = project,
                Id = "Id"
            };
            DataItem projectName = new()
            {
                DataTable = project,
                Id = "Name"
            };
            DataItem taskProjectId = new()
            {
                DataTable = task,
                Id = "ProjectId"
            };
            DataItem taskName = new()
            {
                DataTable = task,
                Id = "Name"
            };
            DataItem taskId = new()
            {
                DataTable = task,
                Id = "Id"
            };
            DataItem resourceId = new()
            {
                DataTable = resource,
                Id = "Id"
            };
            DataItem resourceFullName = new()
            {
                DataTable = resource,
                Id = "FullName"
            };
            DataItem projectProjectManagerId = new()
            {
                DataTable = project,
                Id = "ProjectManagerId"
            };
            #endregion

            Module module = new()
            {
                DataAreas = new[]
                {
                    new DataArea
                    {
                        DataTable = project,
                        Children = new List<DataArea>
                        {
                            new()
                            {
                                DataTable = task,
                                DataAreaReferences = new[]
                                {
                                    new DataAreaReference
                                    {
                                        KeyDataItem = projectId,
                                        ReferenceDataItem = taskProjectId
                                    }
                                },
                                DataFields = new List<DataField>
                                {
                                    new()
                                    {
                                        DataItem = taskId
                                    },
                                    new()
                                    {
                                        DataItem = taskName
                                    },
                                    new()
                                    {
                                        DataItem = projectName
                                    }
                                }
                            },
                            new()
                            {
                                DataTable = resource,
                                DataAreaReferences = new List<DataAreaReference>
                                {
                                    new()
                                    {
                                        KeyDataItem = projectProjectManagerId,
                                        ReferenceDataItem = resourceId
                                    }
                                },
                                DataFields = new List<DataField>
                                {
                                    new()
                                    {
                                        DataItem = resourceFullName
                                    }
                                }
                            }
                        },

                        DataFields = new List<DataField>
                        {
                            new()
                            {
                                DataItem = projectId,
                                FilterFrom = "1"
                            },
                            new()
                            {
                                DataItem = projectName,
                                FilterFrom = "Pr%"
                            }
                        }
                    }
                }
            };
            
            
            
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