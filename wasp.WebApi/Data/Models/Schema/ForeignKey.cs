using System.Collections.Generic;
using System.Linq;

namespace wasp.WebApi.Data.Models.Schema
{
    public class ForeignKey
    {
        public string ForeignKeyConstraintName { get; set; }
        public string ForeignTable { get; set; }
        public string ForeignColumns { get; set; }
        public IEnumerable<string> GetForeignColumns() 
            => ForeignColumns.Split(',').Select(x => x.Trim());
        public string ReferencedTable { get; set; }
        public string ReferencedColumns { get; set; }
        public IEnumerable<string> GetReferencedColumns() 
            => ReferencedColumns.Split(',').Select(x => x.Trim());
    }
}