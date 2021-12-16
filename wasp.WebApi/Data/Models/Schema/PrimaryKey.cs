using System.Collections.Generic;
using System.Linq;

namespace wasp.WebApi.Data.Models.Schema
{
    public class PrimaryKey
    {
        public string SchemaName { get; set; }
        public string PrimaryKeyName { get; set; }
        public string Columns { get; set; }
        
        public IEnumerable<string> GetColumns() 
            => Columns.Split(',').Select(x => x.Trim());

        public string TableName { get; set; }
    }
}