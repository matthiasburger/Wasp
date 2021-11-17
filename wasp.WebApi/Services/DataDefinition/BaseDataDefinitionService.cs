using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Python.Runtime;

using wasp.WebApi.Data.Models;
using wasp.WebApi.Services.DatabaseAccess;

namespace wasp.WebApi.Services.DataDefinition
{
    public abstract class BaseDataDefinitionService : IDataDefinitionService
    {
        private readonly IDatabaseService _databaseService;

        public BaseDataDefinitionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public virtual string GetCreateTableStatement(DataTableDefinition tableDefinition) => $@"
CREATE TABLE [dbo].[{tableDefinition.TableName}](
{string.Join(",\r\n", tableDefinition.PrimaryKeyColumns.Select(x => GetCreatePropertyStatement(x)).ToArray())} ,
    CONSTRAINT [PK_{tableDefinition.TableName}] PRIMARY KEY CLUSTERED (
    {string.Join(",\r\n", tableDefinition.PrimaryKeyColumns.Select(x => $"[{x.Name}] ASC").ToArray())}
    )
)
";
        public virtual string GetCreatePropertyStatement(ColumnDefinition columnDefinition)
            => $@"    [{columnDefinition.Name}] {GetDataType(columnDefinition)} {GetIdentity(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";

        public virtual string GetIdentity(ColumnDefinition columnDefinition)
        {
            if (columnDefinition is not PrimaryKeyColumnDefinition primaryKeyColumn)
                return string.Empty;
            if (primaryKeyColumn.IdentitySeed is null || primaryKeyColumn.IdentityIncrement is null)
                return string.Empty;
            return $"IDENTITY ({primaryKeyColumn.IdentitySeed}, {primaryKeyColumn.IdentityIncrement})";
        }

        public virtual string GetDataType(ColumnDefinition columnDefinition)
        {
            StringBuilder dataTypeBuilder = new StringBuilder()
                .Append($@"[{columnDefinition.DataType}]");

            if (columnDefinition.Length > 0)
                dataTypeBuilder.Append($@"({columnDefinition.Length})");
            else if (columnDefinition.Scale > 0 && columnDefinition.Precision > 0)
                dataTypeBuilder.Append($@"({columnDefinition.Scale}, {columnDefinition.Precision})");

            return dataTypeBuilder.ToString();
        }

        public virtual string GetDefaultValue(string defaultValue) => string.IsNullOrWhiteSpace(defaultValue) ? string.Empty : $"default({defaultValue})";

        public PyObject CreateDataItem(PyDict dataItem)
        {
            using (Py.GIL())
            {
                ColumnDefinition columnDefinition = new(dataItem);
                string columnCreateStatament = GetColumnCreateStatement(dataItem["DataTable"].ToString(), columnDefinition);

                _databaseService.ExecuteQuery(columnCreateStatament);

                return _buildDtpRecord("DT002", dataItem).ToPython();
            }
        }


        public PyObject UpdateDataItem(PyDict dataItem)
        {
            using (Py.GIL())
            {
                ColumnDefinition columnDefinition = new(dataItem);
                string columnUpdateStatament = GetColumnUpdateStatement(dataItem["DataTable"].ToString(), columnDefinition);
                return _buildDtpRecord("DT002", dataItem).ToPython();
            }
        }

        private string GetColumnCreateStatement(string datatable, ColumnDefinition columnDefinition)
        {
            return $@"ALTER TABLE [{datatable}] ADD [{columnDefinition.Name}] {GetDataType(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";
        }

        private string GetColumnUpdateStatement(string datatable, ColumnDefinition columnDefinition)
        {
            return $@"ALTER TABLE [{datatable}] ALTER COLUMN [{columnDefinition.Name}] {GetDataType(columnDefinition)} {(columnDefinition.IsNullable ? "NULL" : "NOT NULL")} {GetDefaultValue(columnDefinition.DefaultValueSql)}";
        }


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

        private DataTableDefinition _buildDataTableDefinition(PyDict datatable, PyTuple primaryKeys)
        {
            List<PrimaryKeyColumnDefinition> primaryKeyColumns = new();

            foreach (PyObject primaryKey in primaryKeys)
            {
                PrimaryKeyColumnDefinition column = new(new PyDict(primaryKey));
                primaryKeyColumns.Add(column);
            }

            return new DataTableDefinition()
            {
                TableName = datatable["SqlId"].ToString(),
                PrimaryKeyColumns = primaryKeyColumns.ToArray()
            };
        }

        private PyTuple _buildDtpRecordsForTableCreation(PyDict datatable, PyTuple primaryKeys)
        {
            DtpRecord dataTableDtp = _buildDtpRecord("DT001", datatable);
            IEnumerable<DtpRecord> primaryKeyDtps = primaryKeys.Select(x => _buildDtpRecord("DT002", x));
            IEnumerable<DtpRecord> relationDtps = primaryKeyDtps.Select(x => _buildPrimaryKeyRelationDtpRecord(x));

            return new PyTuple(

                new PyObject[3]
                {
                        dataTableDtp.ToPython(),
                        new PyTuple(primaryKeyDtps.Select(x => x.ToPython()).ToArray()),
                        new PyTuple(relationDtps.Select(x => x.ToPython()).ToArray()),
                }

            );
        }

        private DtpRecord _buildPrimaryKeyRelationDtpRecord(DtpRecord dtpRecord)
        {
            if (dtpRecord.DataTable is not "DT002")
                throw new ArgumentException("datatable must be DT002 to build relations");

            string source = dtpRecord.DataItems.First(x => x.DataItemId == "SqlId").Value.ToString();
            string datatable = dtpRecord.DataItems.First(x => x.DataItemId == "DataTable").Value.ToString();

            return new DtpRecord
            {
                DataTable = "DT003",
                DataItems = new DataItem[2]
                {
                    new DataItem { DataItemId = "DataItemId", Value = source },
                    new DataItem { DataItemId = "Name", Value = datatable }
                }
            };
        }

        // todo: change this to pydict!
        private DtpRecord _buildDtpRecord(string table, PyObject pyDict)
        {
            IList<DataItem> dataItems = new List<DataItem>();

            using (PyIter iterator = pyDict.GetIterator())
            {
                while (iterator.MoveNext())
                {
                    PyObject key = iterator.Current;
                    PyObject value = pyDict[key];

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
