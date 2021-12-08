using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wasp.WebApi.Data.Entity;
using wasp.WebApi.Data.SurrogateKeyGenerator;

namespace wasp.WebApi.Data.Models
{
    public class DataArea : SurrogateBaseEntity<string, DefaultSurrogateKeyGenerator>, IRecursiveEntity<DataArea, string>
    {
        [Key]
        [Column("Id", Order = 1, TypeName = "nvarchar(10)"), Required]
        public override string Id { get; set; } = null!;

        [Column("ModuleId", Order = 2, TypeName = "nvarchar(10)")]
        public string? ModuleId { get; set; }

        [Column("Name", Order = 3, TypeName = "nvarchar(300)")]
        public string? Name { get; set; }

        [Column("ParentId", Order = 4, TypeName = "nvarchar(10)")]
        public string? ParentId { get; set; }
        
        [ForeignKey("ModuleId")]
        public Module? Module { get; set; }
        
        [ForeignKey("ParentId")]
        public DataArea? Parent { get; set; }
        
        public ICollection<DataField> DataFields { get; set; } = new List<DataField>();
        
        public ICollection<DataArea> Children { get; set; } = new List<DataArea>();
    }
}