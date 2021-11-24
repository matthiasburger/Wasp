using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

using Python.Runtime;

using Wasp.Core.PythonTools;

using wasp.WebApi.Data.Models;
using wasp.WebApi.Exceptions;
using wasp.WebApi.Services.DatabaseAccess;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace wasp.WebApi.Services.DataDefinition
{
    // [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "we use the actual types on python-objects")]
    public abstract class BaseDataDefinitionService : IDataDefinitionService, IDisposable
    {
        private readonly IDatabaseService _databaseService;
        private readonly ServerConnection _serverConnection;
        private readonly SqlConnection _sqlConnection;

        protected BaseDataDefinitionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _sqlConnection = _databaseService.GetNewConnection();
            _serverConnection = new ServerConnection(_sqlConnection);
        }

        /// <summary>
        /// method to create a datatable
        /// </summary>
        /// <param name="datatable">the actual data-table data</param>
        /// <param name="primaryKeys">the primary keys for this data-table</param>
        /// <returns>a tuple of dtp-records (data-table-DTP, (primary-key-DTP,))</returns>
        public void CreateDataTable(PyDict datatable, PyTuple primaryKeys)
        {
            DataTableDefinition dataTableDefinition;
            using (Py.GIL())
            {
                dataTableDefinition = _buildDataTableDefinition(datatable, primaryKeys);
            }

            _serverConnection.Connect();
            Server server = new(_serverConnection);
            Database database = server.Databases[_sqlConnection.Database];

            Table t = new(database, dataTableDefinition.TableName);
            Index idx = new(t, @"PK_" + t.Name)
            {
                IsClustered = false,
                IsUnique = true,
                IndexKeyType = IndexKeyType.DriPrimaryKey
            };
            t.Indexes.Add(idx);

            foreach (PrimaryKeyColumnDefinition column in dataTableDefinition.PrimaryKeyColumns)
            {
                Column keyColumn = new(t, column.SqlId, _getDataType(column))
                {
                    Nullable = column.IsNullable
                };
                if (column.DefaultValueSql is not null)
                    keyColumn.Default = column.DefaultValueSql;

                if (column.IdentityIncrement.HasValue && column.IdentitySeed.HasValue)
                {
                    keyColumn.Identity = true;
                    keyColumn.IdentityIncrement = column.IdentityIncrement.Value;
                    keyColumn.IdentitySeed = column.IdentitySeed.Value;
                }

                t.Columns.Add(keyColumn);
                idx.IndexedColumns.Add(new IndexedColumn(idx, keyColumn.Name));
            }

            #if !DEBUG
            t.Create();
            #else

            StringCollection script = t.Script();
            foreach (string scriptPart in script)
            {
                _databaseService.ExecuteQuery(scriptPart);
            }

            #endif

            //string createTableStatement = GetCreateTableStatement(dataTableDefinition);
            //

            //return _buildDtpRecordsForTableCreation(datatable, primaryKeys);
        }

        private static DataType _getDataType(ColumnDefinition column)
        {
            return column.DataType.ToLower() switch
            {
                "varchar" when column.Length.HasValue => DataType.VarChar(column.Length.Value),
                "varchar" => DataType.VarCharMax,
                "nvarchar" when column.Length.HasValue => DataType.NVarChar(column.Length.Value),
                "nvarchar" => DataType.NVarCharMax,
                "bit" => DataType.Bit,
                "int" => DataType.Int,
                "bigint" => DataType.BigInt,
                "smallint" => DataType.SmallInt,
                "tinyint" => DataType.TinyInt,
                "float" => DataType.Float,
                "decimal" => DataType.Decimal(column.Scale ?? 18, column.Precision ?? 0),
                "numeric" => DataType.Numeric(column.Scale ?? 18, column.Precision ?? 0),
                "UniqueIdentifier" => DataType.UniqueIdentifier,
                "datetime" => DataType.DateTime,
                "datetime2" => DataType.DateTime2(column.Precision ?? 7),
                "date" => DataType.Date,
                "time" => DataType.Time(column.Precision ?? 7),
                @"datetimeoffset" => DataType.DateTimeOffset(column.Precision ?? 7),
                @"smalldatetime" => DataType.SmallDateTime,
                "binary" => DataType.Binary(column.Length ?? 50),
                "varbinary" when column.Length.HasValue => DataType.VarBinary(column.Length.Value),
                "varbinary" => DataType.VarBinaryMax,

                _ => throw new NotImplementedException($"{column.DataType.ToLower()} is not a valid sql-server-datatype")
            };
        }

        public void CreateDataItem(PyDict dataItem)
        {
            ColumnDefinition column;
            using (Py.GIL())
                column = dataItem.MapTo<ColumnDefinition>();

            _serverConnection.Connect();
            Server server = new(_serverConnection);
            Database database = server.Databases[_sqlConnection.Database];

            Table t = database.Tables[column.DataTable];

            Column keyColumn = new(t, column.SqlId, _getDataType(column))
            {
                Nullable = column.IsNullable
            };
            if (column.DefaultValueSql is not null)
                keyColumn.Default = column.DefaultValueSql;

            t.Columns.Add(keyColumn);

            keyColumn.Create();
        }

        public void UpdateDataItem(PyDict dataItem)
        {
            // need to get existing column by sql id and change it's name.. could it work?
            
            ColumnDefinition column;
            using (Py.GIL())
                column = dataItem.MapTo<ColumnDefinition>();

            _serverConnection.Connect();
            Server server = new(_serverConnection);
            Database database = server.Databases[_sqlConnection.Database];

            Table t = database.Tables[column.DataTable];
            Column col = t.Columns[column.SqlId];

            col.DataType = _getDataType(column);
            col.Nullable = column.IsNullable;
            col.Default = column.DefaultValueSql;
            
            col.Alter();
        }

        protected virtual string GetCreateTableStatement(DataTableDefinition tableDefinition) => $@"
CREATE TABLE [dbo].[{tableDefinition.TableName}](
{string.Join(",\r\n", tableDefinition.PrimaryKeyColumns.Select(GetCreatePropertyStatement).ToArray())} ,
    CONSTRAINT [PK_{tableDefinition.TableName}] PRIMARY KEY CLUSTERED (
    {string.Join(",\r\n", tableDefinition.PrimaryKeyColumns.Select(x => $"[{x.Name}] ASC").ToArray())}
    )
)
";

        protected virtual string GetCreatePropertyStatement(ColumnDefinition columnDefinition)
            =>
                $@"    [{columnDefinition.Name}] {GetDataType(columnDefinition)} {GetIdentity(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";

        protected virtual string GetIdentity(ColumnDefinition columnDefinition)
        {
            if (columnDefinition is not PrimaryKeyColumnDefinition primaryKeyColumn)
                return string.Empty;
            if (primaryKeyColumn.IdentitySeed is null || primaryKeyColumn.IdentityIncrement is null)
                return string.Empty;
            return $"IDENTITY ({primaryKeyColumn.IdentitySeed}, {primaryKeyColumn.IdentityIncrement})";
        }

        protected virtual string GetDataType(ColumnDefinition columnDefinition)
        {
            StringBuilder dataTypeBuilder = new StringBuilder()
                .Append($@"[{columnDefinition.DataType}]");

            if (columnDefinition.Length > 0)
                dataTypeBuilder.Append($@"({columnDefinition.Length})");
            else if (columnDefinition.Scale > 0 && columnDefinition.Precision > 0)
                dataTypeBuilder.Append($@"({columnDefinition.Scale}, {columnDefinition.Precision})");

            return dataTypeBuilder.ToString();
        }

        protected virtual string GetDefaultValue(string defaultValue) => string.IsNullOrWhiteSpace(defaultValue) ? string.Empty : $"default({defaultValue})";

        protected virtual string GetColumnCreateStatement(string datatable, ColumnDefinition columnDefinition)
        {
            return
                $@"ALTER TABLE [{datatable}] ADD [{columnDefinition.Name}] {GetDataType(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";
        }

        protected virtual string GetColumnUpdateStatement(string datatable, ColumnDefinition columnDefinition)
        {
            return
                $@"ALTER TABLE [{datatable}] ALTER COLUMN [{columnDefinition.Name}] {GetDataType(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";
        }

        private static DataTableDefinition _buildDataTableDefinition(PyDict datatable, PyTuple primaryKeys)
        {
            if (!datatable.HasKey("SqlId"))
                throw new MissingParameterException<PyDict>(nameof(datatable), "SqlId");

            return new DataTableDefinition
            {
                TableName = datatable["SqlId"].ToString(),
                PrimaryKeyColumns = primaryKeys
                    .Select(primaryKey => new PyDict(primaryKey).MapTo<PrimaryKeyColumnDefinition>())
                    //.Select(primaryKey => new PrimaryKeyColumnDefinition(new PyDict(primaryKey)))
                    .ToArray()
            };
        }

        private static PyTuple _buildDtpRecordsForTableCreation(PyDict datatable, PyTuple primaryKeys)
        {
            DtpRecord dataTableDtp = _buildDtpRecord("DT001", datatable);
            IEnumerable<DtpRecord> primaryKeyDtpRecords = primaryKeys.Select(x => _buildDtpRecord("DT002", new PyDict(x))).ToList();
            IEnumerable<DtpRecord> relationDtpRecords = primaryKeyDtpRecords.Select(_buildPrimaryKeyRelationDtpRecord);

            return new PyTuple(
                new[]
                {
                    dataTableDtp.ToPython(),
                    new PyTuple(primaryKeyDtpRecords.Select(x => x.ToPython()).ToArray()),
                    new PyTuple(relationDtpRecords.Select(x => x.ToPython()).ToArray()),
                }
            );
        }

        private static DtpRecord _buildPrimaryKeyRelationDtpRecord(DtpRecord dtpRecord)
        {
            if (dtpRecord.DataTable is not "DT002")
                throw new ArgumentException("datatable must be DT002 to build relations");

            string source = dtpRecord.DataItems.First(x => x.DataItemId == "SqlId").Value.ToString();
            string datatable = dtpRecord.DataItems.First(x => x.DataItemId == "DataTable").Value.ToString();

            return new DtpRecord
            {
                DataTable = "DT003",
                DataItems = new DataItem[]
                {
                    new() { DataItemId = "DataItemId", Value = source },
                    new() { DataItemId = "Name", Value = datatable }
                }
            };
        }

        private static DtpRecord _buildDtpRecord(string table, PyDict pyDict)
        {
            IList<DataItem> dataItems = (
                from key in pyDict.Keys()
                let value = pyDict[key]
                select new DataItem { DataItemId = key.ToString(), Value = value }
            ).ToList();

            return new DtpRecord
            {
                DataTable = table,
                DataItems = dataItems.ToArray()
            };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sqlConnection.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}