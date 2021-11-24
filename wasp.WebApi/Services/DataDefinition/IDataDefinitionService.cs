using Python.Runtime;

namespace wasp.WebApi.Services.DataDefinition
{
    public interface IDataDefinitionService
    {
        //PyTuple CreateDataTable(PyDict datatable, PyTuple primaryKeys);
        void CreateDataTable(PyDict datatable, PyTuple primaryKeys);
        //PyObject CreateDataItem(PyDict dataItem);
        void CreateDataItem(PyDict dataItem);
        void UpdateDataItem(PyDict dataItem);
    }
}
