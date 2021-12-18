using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

using IronSphere.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using SqlKata.Compilers;

using wasp.WebApi.Data;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.Mts;

using DataTable = wasp.WebApi.Data.Models.DataTable;

namespace wasp.WebApi.Controllers
{
    public struct Equa : IEqualityComparer<Equa>
    {
        public object[] Vals { get; set; }

        public bool Equals(Equa a, Equa b)
        {
            var y =  !a.Vals.Where((t, i) => b.Vals[i] != t).Any();
            return y;
        }

        public int GetHashCode(Equa a)
        {
            var x = a.Vals.Aggregate(17, (current, evt) => current * 31 + evt.GetHashCode());
            return x;
        }
    }
    
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
            DataItem resourceFirstName = new()
            {
                DataTable = resource,
                Id = "FirstName"
            };
            DataItem resourceLastName = new()
            {
                DataTable = resource,
                Id = "LastName"
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
                                        DataItem = resourceFirstName
                                    },
                                    new()
                                    {
                                        DataItem = resourceLastName
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
                            }
                        }
                    }
                }
            };

            QueryBuilder? queryBuilder = module.DataAreas.First().As<IDataArea>()?.BuildQuery();
            var (sql, parameters) = queryBuilder.GetQuery();
            
            await using SqlConnection connection = new (@"Server=(localdb)\mssqllocaldb;Database=wasp;Trusted_Connection=True;MultipleActiveResultSets=true");
            await using SqlCommand cmd = new SqlCommand(sql, connection).SetParameters(parameters);

            connection.Open();

            MtsModule mtsModule = new ();
            System.Data.DataTable dt = new();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
            {
                dataAdapter.Fill(dt);
            }

            IEnumerable<DatabaseResult> result = dt.Rows.Cast<DataRow>().Select(x => new DatabaseResult(x)).ToList();

            foreach (DataArea moduleDataArea in module.DataAreas)
            {
                mtsModule.DataAreas.Add(_buildDataArea(moduleDataArea, result));
            }
            
            string serMts = JsonConvert.SerializeObject(mtsModule);

            return Ok(mtsModule);
        }

        private MtsDataArea _buildDataArea(DataArea moduleDataArea, IEnumerable<DatabaseResult> result)
        {
            MtsDataArea dataArea = new ();
            
            int[] ordinals = moduleDataArea.DataFields.Select(x => x.Ordinal).ToArray();
            IEnumerable<IGrouping<object, DatabaseResult>> gr = result.GroupBy(x => x.GetGrouping(ordinals));

            foreach (IGrouping<object,DatabaseResult> grouping in gr)
            {
                IEnumerable<DatabaseResult> nextResult = grouping;
                DatabaseResult key = grouping.First();

                MtsRecord mtsRecord = new MtsRecord();
                
                dataArea.Records.Add(mtsRecord);

                foreach (DataArea subArea in moduleDataArea.Children)
                {
                    mtsRecord.DataArea.Add(_buildDataArea(subArea, nextResult));
                }
                
                foreach (DataField dfs in moduleDataArea.DataFields)
                {
                    int ordinal = dfs.Ordinal;
                    DataItem item = dfs.DataItem;
                    MtsDataField field = key.GetDataField(ordinal);
                    field.Name = item.Id;
                    mtsRecord.DataFields.Add(field);
                }
            }

            return dataArea;
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

    public class DatabaseResult : IEqualityComparer<DatabaseResult>
    {
        private readonly IEnumerable<MtsDataField> _dataFields;

        public DatabaseResult(DataRow dataRow)
        {
            int columnLength = dataRow.Table.Columns.Count;

            MtsDataField[] dataFields = new MtsDataField[columnLength];

            for (int columnIndex = 0; columnIndex < columnLength; columnIndex++)
            {
                dataFields[columnIndex] = new MtsDataField
                {
                    Value = dataRow[columnIndex],
                    Ordinal = columnIndex
                };
            }

            _dataFields = dataFields;
        }

        private int[] _ordinals;
        
        public object GetGrouping(int[] ordinals)
        {
            _ordinals = ordinals;
            
            return T();
        }

        public string T()
        {
            return string.Join("-", _dataFields.Where(w => _ordinals.Contains(w.Ordinal)).Select(s => s.Value));
        }

        public MtsDataField GetDataField(int ordinal)
        {
            return _dataFields.First(x => x.Ordinal == ordinal);
        }
        
        public bool Equals(DatabaseResult? x, DatabaseResult? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            
            return x.GetType() == y.GetType() 
                   && x.T().Equals(y.T());
        }

        public int GetHashCode(DatabaseResult obj)
        {
            return obj.T().GetHashCode();
        }
    }
}