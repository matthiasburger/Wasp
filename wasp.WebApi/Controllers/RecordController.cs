using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SqlKata;
using SqlKata.Compilers;
using wasp.WebApi.Controllers.Base;
using wasp.WebApi.Data;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.Mts;
using DataTable = wasp.WebApi.Data.Models.DataTable;

namespace wasp.WebApi.Controllers
{
    public class RecordCreationRequest
    {
        public MtsDataAreaInfo DataAreaInfo { get; set; }
        public MtsRecord? Parent { get; set; }
    }
    
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

        [HttpPost, Route("save_record")]
        public async Task SaveRecord(MtsRecord record)
        {
            await _saveMtsRecord(record, false);
        }

        [HttpPost, Route("save_module")]
        public async Task SaveModule(MtsModule module)
        {
            foreach (MtsDataArea mtsDataArea in module.DataAreas)
                await _saveMtsDataArea(mtsDataArea);
        }

        [HttpPost, Route("create_record")]
        public async Task<IActionResult> CreateRecord(RecordCreationRequest request)
        {
            DataArea dataArea = await _dbContext.DataAreas
                .Include(x => x.DataAreaReferences)
                .ThenInclude(x => x.KeyDataItem)
                .ThenInclude(x => x.DataTable)
                .Include(x => x.DataAreaReferences)
                .ThenInclude(x => x.ReferenceDataItem)
                .ThenInclude(x => x.DataTable)
                .Include(x => x.DataTable)
                .Include(x => x.DataFields)
                .ThenInclude(x => x.DataItem)
                .SingleAsync(x => x.Id == request.DataAreaInfo.Id);

            return EnvelopeResult.Ok(_buildRecord(dataArea, request.Parent));
        }

        private static MtsRecord _buildRecord(IDataArea moduleDataArea, MtsRecord? parent)
        {
            MtsRecord mtsRecord = new()
            {
                DataTableId = moduleDataArea.DataTable.Id,
                NewRecord = true
            };

            foreach (DataField dfs in moduleDataArea.DataFields)
            {
                DataItem? item = dfs.DataItem;
                MtsDataField mtsDataField = new()
                {
                    DataItemInfo = item?.GetDataItemInfo(),
                    Name = item?.Id ?? "<DataItem not found>"
                };
                MtsDataField? pDf = parent?.DataFields.FirstOrDefault(x => 
                    x.DataItemInfo?.Id == dfs.DataItemId 
                    && x.DataItemInfo?.DataTableId == dfs.DataTableId
                );
                if (pDf != null)
                {
                    mtsDataField.Value = pDf.Value;
                }

                mtsRecord.DataFields.Add(mtsDataField);
            }

            foreach (DataField dfs in moduleDataArea.DataFields)
            {
                IEnumerable<(DataItem ReferenceDataItem, DataItem KeyDataItem)> x = dfs.DataArea.DataAreaReferences
                    .Where(w=>w.KeyDataItemId == dfs.DataItemId && w.KeyDataItemDataTableId == dfs.DataTableId)
                    .Select(c=>(c.ReferenceDataItem, c.KeyDataItem));
                foreach ((DataItem referenceDataItem, DataItem keyDataItem) in x)
                {
                    IEnumerable<MtsDataField> mtsDataFields = mtsRecord.DataFields.Where(z => z.DataItemInfo.DataTableId == keyDataItem.DataTableId && z.DataItemInfo.Id == keyDataItem.Id);

                    foreach (MtsDataField mtsDataField in mtsDataFields)
                    {
                        MtsDataField? pDf = parent?.DataFields.FirstOrDefault(y => 
                            y.DataItemInfo?.Id == referenceDataItem.Id 
                            && y.DataItemInfo?.DataTableId == referenceDataItem.DataTableId
                        );
                    
                        if (pDf != null)
                        {
                            mtsDataField.Value = pDf.Value;
                        }
                    }
                }
            }
            

            return mtsRecord;
        }

        private async Task _saveMtsDataArea(MtsDataArea mtsDataArea)
        {
            foreach (MtsRecord mtsRecord in mtsDataArea.Records)
                await _saveMtsRecord(mtsRecord, true);
        }

        private async Task _saveMtsRecord(MtsRecord mtsRecord, bool saveChildren)
        {
            if (saveChildren)
            {
                foreach (MtsDataArea mtsDataArea in mtsRecord.DataAreas)
                    await _saveMtsDataArea(mtsDataArea);
            }

            if (!mtsRecord.UnsavedChanges)
                return;

            DataTable dataTable = await _dbContext.DataTables
                .Include(x => x.DataItems)
                .ThenInclude(x => x.KeyRelations)
                .ThenInclude(x => x.Index)
                .SingleAsync(x => x.Id == mtsRecord.DataTableId);


            Query q = new(dataTable.SqlId);
            IEnumerable<MtsDataField> dataFieldsToUpdate = mtsRecord.DataFields
                .Where(w => w.DataItemInfo?.DataTableId == mtsRecord.DataTableId)
                .ToList();

            var x = (from dataField in dataFieldsToUpdate
                join dataItem in dataTable.DataItems on dataField.DataItemInfo.Id equals dataItem.Id
                where dataField.Value != null
                select new
                {
                    sqlId = dataItem.Id,
                    isPrimaryKey = dataItem.KeyRelations.Any(x => x.Index.Type == IndexType.PrimaryKey),
                    value = dataField.Value
                }).ToList();

            if (mtsRecord.NewRecord)
            {
                q.AsInsert(x.Select(y => y.sqlId), x.Select(y => y.value));
            }
            else
            {
                var columnsToUpdate = x.Where(w => !w.isPrimaryKey).ToList();
                var referenceColumns = x.Where(w => w.isPrimaryKey).ToList();

                q.AsUpdate(columnsToUpdate.Select(y => y.sqlId), columnsToUpdate.Select(y => y.value));
                foreach (var referenceColumn in referenceColumns)
                    q.Where(referenceColumn.sqlId, referenceColumn.value);
            }
            
            await _executeQuery(q);
        }

        private async Task _executeQuery(Query query)
        {
            SqlServerCompiler compiler = new();
            SqlResult? sqlResult = compiler.Compile(query);

            await using SqlConnection connection =
                new(_configuration.GetConnectionString("WaspSqlServerConnectionString"));
            await using SqlCommand cmd = new SqlCommand(sqlResult.Sql, connection)
                .SetParameters(sqlResult.NamedBindings);

            connection.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}