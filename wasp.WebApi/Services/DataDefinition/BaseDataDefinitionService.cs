using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using Python.Runtime;

using wasp.Core.PythonTools.PyMap;
using wasp.WebApi.Exceptions;
using wasp.WebApi.Services.DatabaseAccess;

using Index = Microsoft.SqlServer.Management.Smo.Index;
using SqlCommand = System.Data.SqlClient.SqlCommand;

namespace wasp.WebApi.Services.DataDefinition
{
    public class BaseDataDefinitionService : IDataDefinitionService, IDisposable
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
        /// <param name="columns">the columns for this data-table</param>
        /// <returns>a tuple of dtp-records (data-table-DTP, (primary-key-DTP,))</returns>
        public void CreateDataTable(PyDict datatable, PyTuple primaryKeys, PyList? columns = null)
        {
            DataTableDefinition dataTableDefinition;
            using (Py.GIL())
                dataTableDefinition = _buildDataTableDefinition(datatable, primaryKeys, columns);

            _serverConnection.Connect();
            Server server = new(_serverConnection);
            Database database = server.Databases[_sqlConnection.Database];

            Table t = new(database, dataTableDefinition.TableName);
            Index idx = new(t, $@"PK_{t.Name}")
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

            foreach (ColumnDefinition column in dataTableDefinition.Columns)
            {
                Column dbColumn = new(t, column.SqlId, _getDataType(column))
                {
                    Nullable = column.IsNullable
                };
                if (column.DefaultValueSql is not null)
                    dbColumn.Default = column.DefaultValueSql;

                t.Columns.Add(dbColumn);
            }

            t.Create();
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
            col.Default = column.DefaultValueSql ?? string.Empty;
            
            col.Alter();
        }

        public void CreateDtpRecord(string datatable, PyDict properties)
        {
            using Py.GILState _ = Py.GIL();

            PyIterable keys = properties.Keys();

            string query = $@"insert into [{datatable}] ({string.Join(", ", keys.Select(x => $"[{x}]").ToArray())}) values ({keys.Select(x => $"@{x}").ToArray()})";

            using SqlCommand command = _databaseService.CreateSqlCmd(query);
            command.Parameters.AddRange(_generateParameters(properties).ToArray());
                
            command.ExecuteNonQuery();
        }

        private static IEnumerable<SqlParameter> _generateParameters(PyDict properties)
        {
            return from pyKey in properties.Keys() 
                    let key = pyKey.ToString() 
                    let value = properties[pyKey].AsManagedObject(typeof(object)) 
                    select new SqlParameter
                    {
                        ParameterName = key,
                        Value = value ?? DBNull.Value
                    };
        }

        private static DataType _getDataType(ColumnDefinition column)
        {
            if (column.DataType is null)
                throw new MissingParameterException<ColumnDefinition>(nameof(column), "DataType");
            
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

        private static DataTableDefinition _buildDataTableDefinition(PyDict datatable, PyTuple primaryKeys, PyList? columns)
        {
            DataTableDefinition definition = datatable.MapTo<DataTableDefinition>();
            definition.AddPrimaryKeys(primaryKeys);
            if (columns != null)
                definition.AddColumns(columns);
            return definition;
        }

        private void _dispose(bool disposing)
        {
            if (disposing)
            {
                _sqlConnection.Dispose();
            }
        }

        public void Dispose()
        {
            _dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}