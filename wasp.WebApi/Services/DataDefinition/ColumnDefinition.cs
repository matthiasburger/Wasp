
using Python.Runtime;

namespace wasp.WebApi.Services.DataDefinition
{
    public class ColumnDefinition
    {
        public ColumnDefinition(PyDict dataItem)
        {
            Name = dataItem["SqlId"].ToString();
            DataType = dataItem["DbType"].ToString();
            Length = dataItem.HasKey("DbLength") ? dataItem["DbLength"].As<int>() : null;
            Precision = dataItem.HasKey("DbPrecision") ? dataItem["DbPrecision"].As<int>() : null;
            Scale = dataItem.HasKey("DbScale") ? dataItem["DbScale"].As<int>() : null;

        }

        public string Name { get; set; }
        public string DataType { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsNullable { get; set; }
        public string DefaultValueSql { get; set; }


    }
}
