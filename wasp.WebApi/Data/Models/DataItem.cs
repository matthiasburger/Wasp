using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wasp.WebApi.Data.Models
{
    [Table("DataItem")]
    public class DataItem
    {
        [Column("Id", Order = 0, TypeName = "nvarchar(300)")]
        // [Key]
        public string Id { get; set; }

        [Column("DataTableId", Order = 1, TypeName = "nvarchar(100)")]
        // [Key]
        public string DataTableId { get; set; }
        
        [Column("Name", Order = 2, TypeName = "nvarchar(400)")]
        public string Name { get; set; }

        [Column("PythonId", Order = 3, TypeName = "nvarchar(100)")]
        public string PythonId { get; set; }
        
        
        [ForeignKey("DataTableId")]
        public DataTable DataTable { get; set; }

        public IEnumerable<Relation> KeyRelations { get; set; }
        public IEnumerable<Relation> ReferenceRelations { get; set; }
    }
}