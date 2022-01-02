﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using wasp.WebApi.Controllers.Base;
using wasp.WebApi.Data;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.Mts;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ModuleController : ApiBaseController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public ModuleController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Open(string id)
        {
            Module _ = await _dbContext.Modules
                .Include(x => x.DataAreas)
                .ThenInclude(x => x.DataFields)
                .ThenInclude(x => x.DataItem)
                .SingleAsync(x => x.Id == id);

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
                .SingleAsync(x => x.Id == "000000");

            IList<MtsModule> moduleResults = new List<MtsModule>();
            foreach (IDataArea dataArea in module.DataAreas.Where(w => w.Parent is null))
            {
                QueryBuilder? queryBuilder = dataArea.BuildQuery();
                if (queryBuilder is null)
                    throw new Exception("could not build query :(");

                moduleResults.Add(await _getResultsFromQuery(queryBuilder, module));
            }

            return EnvelopeResult.Ok(moduleResults);
        }

        private async Task<MtsModule> _getResultsFromQuery(QueryBuilder queryBuilder, Module module)
        {
            (string sql, IDictionary<string, object> parameters) = queryBuilder.GetQuery();

            await using SqlConnection connection =
                new(_configuration.GetConnectionString("WaspSqlServerConnectionString"));
            await using SqlCommand cmd = new SqlCommand(sql, connection).SetParameters(parameters);

            connection.Open();

            MtsModule mtsModule = new();
            System.Data.DataTable dt = new();
            using (SqlDataAdapter dataAdapter = new(cmd))
            {
                dataAdapter.Fill(dt);
            }

            IEnumerable<DatabaseResult>
                result = dt.Rows.Cast<DataRow>().Select(x => new DatabaseResult(x)).ToList();

            foreach (DataArea moduleDataArea in module.DataAreas.Where(w => w.Parent is null))
            {
                mtsModule.DataAreas.Add(_buildDataArea(moduleDataArea, result));
            }

            string _ = JsonConvert.SerializeObject(mtsModule);

            return mtsModule;
        }

        [Obsolete("the grouping needs to get a refactoring")]
        private static IEnumerable<IGrouping<object, DatabaseResult>> _getGrouping(IEnumerable<DatabaseResult> result, int[] ordinals)
        {
            return result.GroupBy(x => x.GetGrouping(ordinals));
        }

        private static MtsDataArea _buildDataArea(IDataArea moduleDataArea, IEnumerable<DatabaseResult> result)
        {
            MtsDataArea dataArea = new()
            {
                DataAreaInfo = moduleDataArea.GetDataAreaInfo()
            };

            int[] ordinals = moduleDataArea.DataFields.Select(x => x.Ordinal).ToArray();
            IEnumerable<IGrouping<object, DatabaseResult>> gr = _getGrouping(result, ordinals);

            foreach (IGrouping<object, DatabaseResult> grouping in gr)
            {
                IEnumerable<DatabaseResult> nextResult = grouping;
                DatabaseResult key = grouping.First();

                MtsRecord mtsRecord = new();
                mtsRecord.DataTableId = moduleDataArea.DataTable.Id;
                foreach (DataArea subArea in moduleDataArea.Children)
                {
                    mtsRecord.DataAreas.Add(_buildDataArea(subArea, nextResult));
                }

                foreach (DataField dfs in moduleDataArea.DataFields)
                {
                    DataItem? item = dfs.DataItem;
                    MtsDataField field = key.GetDataField(dfs.Ordinal);
                    field.DataItemInfo = item?.GetDataItemInfo();
                    field.Name = item?.Id ?? "<DataItem not found>";
                    mtsRecord.DataFields.Add(field);
                }
                dataArea.Records.Add(mtsRecord);

            }

            return dataArea;
        }


        [HttpPost]
        public async Task<IActionResult> Create()
        {
            Module module = new()
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
                    Value = dataRow[columnIndex].ToString() ?? string.Empty,
                    Ordinal = columnIndex
                };
            }

            _dataFields = dataFields;
        }

        private int[]? _ordinals;

        public object GetGrouping(int[] ordinals)
        {
            _ordinals = ordinals;
            return _group();
        }

        private string _group()
        {
            if (_ordinals is null)
                return string.Empty;

            IEnumerable<object> x = _dataFields
                .Where(w => _ordinals.Contains(w.Ordinal))
                .OrderBy(x => x.Ordinal)
                .Select(s => s.Value);

            return string.Join("-", x);
        }

        public MtsDataField GetDataField(int ordinal)
            => _dataFields.First(x => x.Ordinal == ordinal);

        public bool Equals(DatabaseResult? x, DatabaseResult? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.GetType() == y.GetType()
                   && x._group().Equals(y._group());
        }

        public int GetHashCode(DatabaseResult obj)
        {
            return obj._group().GetHashCode();
        }
    }
}