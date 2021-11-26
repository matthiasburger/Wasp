using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Python.Runtime;

using wasp.Core.PythonTools.PyMap;
using wasp.WebApi.Exceptions;

namespace wasp.WebApi.Services.DataDefinition
{
    public class DataTableDefinition
    {
        public DataTableDefinition()
        {
        }

        private DataTableDefinition(PyDict dataTableData)
        {
            if (!dataTableData.HasKey("SqlId"))
                throw new MissingParameterException<PyDict>(nameof(dataTableData), "SqlId");

            TableName = dataTableData["SqlId"].ToString();
        }

        public DataTableDefinition(PyDict dataTableData, PyTuple primaryKeys) : this(dataTableData)
        {
            AddPrimaryKeys(primaryKeys);
        }

        public void AddPrimaryKeys(PyTuple primaryKeys)
        {
            PrimaryKeyColumns.AddRange(
                primaryKeys.Select(primaryKey => new PyDict(primaryKey).MapTo<PrimaryKeyColumnDefinition>())
            );
        }

        [PyMapProperty("SqlId")]
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "set-accessor is used by reflection")]
        public string TableName { get; set; }

        [PyMapIgnore] 
        public List<PrimaryKeyColumnDefinition> PrimaryKeyColumns { get; } = new();

        [PyMapIgnore] 
        public List<ColumnDefinition> Columns { get; } = new();

        public void AddColumns(PyList columns)
        {
            Columns.AddRange(
                    columns.Select(column => new PyDict(column).MapTo<ColumnDefinition>())
                );
        }
    }
}