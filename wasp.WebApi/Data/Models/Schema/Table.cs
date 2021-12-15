using System.ComponentModel.DataAnnotations.Schema;

namespace wasp.WebApi.Data.Models.Schema
{
    public class Table
    {
        [Column("TABLE_CATALOG")]
        public string TableCatalog { get; set; } = null!;
        
        [Column("TABLE_SCHEMA")]
        public string TableSchema { get; set; } = null!;
        
        [Column("TABLE_NAME")]
        public string TableName { get; set; } = null!;
        
        [Column("TABLE_TYPE")]
        public string TableType { get; set; } = null!;
    }
}