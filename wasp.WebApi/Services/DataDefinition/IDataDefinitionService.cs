using Python.Runtime;

namespace wasp.WebApi.Services.DataDefinition
{
    public interface IDataDefinitionService
    {
        void CreateDataTable(PyDict datatable, PyTuple primaryKeys, PyList? columns = null);
        void CreateDataItem(PyDict dataItem);
        void UpdateDataItem(PyDict dataItem);
        void CreateDtpRecord(string datatable, PyDict properties);
    }
}
