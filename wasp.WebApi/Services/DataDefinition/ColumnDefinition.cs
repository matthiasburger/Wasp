using wasp.Core.PythonTools.PyMap;

namespace wasp.WebApi.Services.DataDefinition
{
    public class ColumnDefinition
    {
        public string? SqlId { get; set; }
        public string? Name { get; set; }
        
        [PyMapProperty("DataTable")]
        public string? DataTable { get; set; }
        
        [PyMapProperty("DbType")]
        public string? DataType { get; set; }
        
        [PyMapProperty("DbLength")]
        public int? Length { get; set; }
        
        [PyMapProperty("DbPrecision")]
        public int? Precision { get; set; }
        
        [PyMapProperty("DbScale")]
        public int? Scale { get; set; }
        
        public bool IsNullable { get; set; }
        
        public string? DefaultValueSql { get; set; }
    }
}
