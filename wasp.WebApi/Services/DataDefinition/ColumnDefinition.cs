using Wasp.Core.PythonTools;

namespace wasp.WebApi.Services.DataDefinition
{
    public class ColumnDefinition
    {
        public string SqlId { get; set; }
        public string Name { get; set; }
        
        [PyProperty("DataTable")]
        public string DataTable { get; set; }
        
        [PyProperty("DbType")]
        public string DataType { get; set; }
        
        [PyProperty("DbLength")]
        public int? Length { get; set; }
        
        [PyProperty("DbPrecision")]
        public int? Precision { get; set; }
        
        [PyProperty("DbScale")]
        public int? Scale { get; set; }
        
        public bool IsNullable { get; set; }
        
        public string DefaultValueSql { get; set; }
    }
}
