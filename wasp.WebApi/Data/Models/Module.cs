using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wasp.WebApi.Data.SurrogateKeyGenerator;

namespace wasp.WebApi.Data.Models
{
    public class Module : SurrogateBaseEntity<string, DefaultSurrogateKeyGenerator>
    {
        [Key]
        [Column("Id", Order = 1, TypeName = "nvarchar(10)"), Required]
        public override string Id { get; set; } = null!;

        
        [Required]
        public string Name { get; set; } = null!;

        public ICollection<DataArea> DataAreas { get; set; } = new List<DataArea>();
    }
}