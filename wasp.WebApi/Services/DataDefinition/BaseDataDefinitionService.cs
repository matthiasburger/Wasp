using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using Python.Runtime;

using wasp.WebApi.Data.Models;
using wasp.WebApi.Exceptions;
using wasp.WebApi.Services.DatabaseAccess;

namespace wasp.WebApi.Services.DataDefinition
{
    // [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "we use the actual types on python-objects")]
    public abstract class BaseDataDefinitionService : IDataDefinitionService
    {
        private readonly IDatabaseService _databaseService;

        protected BaseDataDefinitionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        /// <summary>
        /// method to create a datatable
        /// </summary>
        /// <param name="datatable">the actual data-table data</param>
        /// <param name="primaryKeys">the primary keys for this data-table</param>
        /// <returns>a tuple of dtp-records (data-table-DTP, (primary-key-DTP,))</returns>
        public PyTuple CreateDataTable(PyDict datatable, PyTuple primaryKeys)
        {
            using (Py.GIL())
            {
                DataTableDefinition dataTableDefinition = _buildDataTableDefinition(datatable, primaryKeys);

                string createTableStatement = GetCreateTableStatement(dataTableDefinition);
                _databaseService.ExecuteQuery(createTableStatement);

                return _buildDtpRecordsForTableCreation(datatable, primaryKeys);
            }
        }
        
        public PyObject CreateDataItem(PyDict dataItem)
        {
            using (Py.GIL())
            {
                ColumnDefinition columnDefinition = new(dataItem);
                string columnCreateStatement = GetColumnCreateStatement(dataItem["DataTable"].ToString(), columnDefinition);

                _databaseService.ExecuteQuery(columnCreateStatement);

                return _buildDtpRecord("DT002", dataItem).ToPython();
            }
        }
        
        public PyObject UpdateDataItem(PyDict dataItem)
        {
            using (Py.GIL())
            {
                ColumnDefinition columnDefinition = new(dataItem);
                string _ = GetColumnUpdateStatement(dataItem["DataTable"].ToString(), columnDefinition);
                return _buildDtpRecord("DT002", dataItem).ToPython();
            }
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
            => $@"    [{columnDefinition.Name}] {GetDataType(columnDefinition)} {GetIdentity(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";

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
            return $@"ALTER TABLE [{datatable}] ADD [{columnDefinition.Name}] {GetDataType(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";
        }

        protected virtual string GetColumnUpdateStatement(string datatable, ColumnDefinition columnDefinition)
        {
            return $@"ALTER TABLE [{datatable}] ALTER COLUMN [{columnDefinition.Name}] {GetDataType(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";
        }

        private static DataTableDefinition _buildDataTableDefinition(PyDict datatable, PyTuple primaryKeys)
        {
            if (!datatable.HasKey("SqlId"))
                throw new MissingParameterException<PyDict>(nameof(datatable), "SqlId");
                
            return new DataTableDefinition
            {
                TableName = datatable["SqlId"].ToString(),
                PrimaryKeyColumns = primaryKeys
                                        .Select(primaryKey => new PrimaryKeyColumnDefinition(new PyDict(primaryKey)))
                                        .ToArray()
            };
        }

        private static PyTuple _buildDtpRecordsForTableCreation(PyDict datatable, PyTuple primaryKeys)
        {
            DtpRecord dataTableDtp = _buildDtpRecord("DT001", datatable);
            IEnumerable<DtpRecord> primaryKeyDtpRecords = primaryKeys.Select(x => _buildDtpRecord("DT002", new PyDict(x))).ToList();
            IEnumerable<DtpRecord> relationDtpRecords = primaryKeyDtpRecords.Select(_buildPrimaryKeyRelationDtpRecord);

            return new PyTuple(
                new []
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

        [Obsolete("use _buildDtpRecord(string,PyDict), will be removed")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static DtpRecord _buildDtpRecord(string table, PyObject pyObject)
        {
            IList<DataItem> dataItems = new List<DataItem>();

            using (PyIter iterator = pyObject.GetIterator())
            {
                while (iterator.MoveNext())
                {
                    PyObject key = iterator.Current;
                    if (key is null) continue;
                    
                    PyObject value = pyObject[key];

                    dataItems.Add(new DataItem { DataItemId = key.ToString(), Value = value });
                }
            }

            return new DtpRecord
            {
                DataTable = table,
                DataItems = dataItems.ToArray()
            };
        }
    }
}
