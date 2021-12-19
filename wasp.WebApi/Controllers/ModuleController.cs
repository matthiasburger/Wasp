using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using IronSphere.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using wasp.WebApi.Controllers.Base;
using wasp.WebApi.Data;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.Mts;

using DataTable = wasp.WebApi.Data.Models.DataTable;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ModuleController : ApiBaseController
    {
        private readonly ApplicationDbContext _dbContext;
        
        public ModuleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> Open(string id)
        {
            Module _ = await _dbContext.Modules
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataFields)
                .ThenInclude(x => x.DataItem)
                .SingleAsync(x=>x.Id == id);
            
            return Ok(new { ok = true, called_at = DateTime.Now });
        }
        
        [HttpGet("demo")]
        public async Task<ActionResult> OpenDemo()
        {
            Module module = await _dbContext.Modules
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataAreaReferences)
                .ThenInclude(x => x.KeyDataItem)
                .ThenInclude(x => x.DataTable)
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataAreaReferences)
                .ThenInclude(x => x.ReferenceDataItem)
                .ThenInclude(x => x.DataTable)
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataTable)
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataFields)
                .ThenInclude(x => x.DataItem)
                .SingleAsync(x=>x.Id == "000000");
            
            QueryBuilder? queryBuilder = module.DataAreas.First(x => x.Parent == null).As<IDataArea>()?.BuildQuery();
            if (queryBuilder is null)
                throw new Exception("could not build query :(");
            
            (string sql, IDictionary<string, object> parameters) = queryBuilder.GetQuery();
            
            await using SqlConnection connection = new (@"Server=(localdb)\mssqllocaldb;Database=wasp;Trusted_Connection=True;MultipleActiveResultSets=true");
            await using SqlCommand cmd = new SqlCommand(sql, connection).SetParameters(parameters);

            connection.Open();

            MtsModule mtsModule = new ();
            System.Data.DataTable dt = new();
            using (SqlDataAdapter dataAdapter = new (cmd))
            {
                dataAdapter.Fill(dt);
            }

            IEnumerable<DatabaseResult> result = dt.Rows.Cast<DataRow>().Select(x => new DatabaseResult(x)).ToList();

            foreach (DataArea moduleDataArea in module.DataAreas.Where(w=>w.Parent is null))
            {
                mtsModule.DataAreas.Add(_buildDataArea(moduleDataArea, result));
            }
            
            string _ = JsonConvert.SerializeObject(mtsModule);

            return EnvelopeResult.Ok(mtsModule);
        }

        private static MtsDataArea _buildDataArea(IDataArea moduleDataArea, IEnumerable<DatabaseResult> result)
        {
            MtsDataArea dataArea = new()
            {
                DataAreaInfo = moduleDataArea.GetDataAreaInfo()
            };
            
            int[] ordinals = moduleDataArea.DataFields.Select(x => x.Ordinal).ToArray();
            IEnumerable<IGrouping<object, DatabaseResult>> gr = result.GroupBy(x => x.GetGrouping(ordinals));

            foreach (IGrouping<object,DatabaseResult> grouping in gr)
            {
                IEnumerable<DatabaseResult> nextResult = grouping;
                DatabaseResult key = grouping.First();

                MtsRecord mtsRecord = new ();
                
                dataArea.Records.Add(mtsRecord);

                foreach (DataArea subArea in moduleDataArea.Children)
                {
                    mtsRecord.DataAreas.Add(_buildDataArea(subArea, nextResult));
                }
                
                foreach (DataField dfs in moduleDataArea.DataFields)
                {
                    int ordinal = dfs.Ordinal;
                    DataItem? item = dfs.DataItem;
                    MtsDataField field = key.GetDataField(ordinal);
                    field.DataItemInfo = item?.GetDataItemInfo();
                    field.Name = item?.Id ?? "<DataItem not found>";
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

        private int[]? _ordinals;
        
        public object GetGrouping(int[] ordinals)
        {
            _ordinals = ordinals;
            
            return T();
        }

        public string T()
        {
            return _ordinals is not null 
                ? string.Join("-", _dataFields.Where(w => _ordinals.Contains(w.Ordinal)).OrderBy(x => x.Ordinal).Select(s => s.Value))
                : string.Empty;

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