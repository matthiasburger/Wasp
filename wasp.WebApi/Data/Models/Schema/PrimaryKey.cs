using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace wasp.WebApi.Data.Models.Schema
{
    public class PrimaryKey
    {
        [Column("schema_name")]
        public string SchemaName { get; set; }
        
        [Column("pk_name")]
        public string PrimaryKeyName { get; set; }
        
        [Column("columns")]
        public string Columns { get; set; }
        
        public IEnumerable<string> GetColumns() 
            => Columns.Split(',').Select(x => x.Trim());

        [Column("table_name")]
        public string TableName { get; set; }
    }
}