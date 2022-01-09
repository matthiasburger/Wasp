using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wasp.WebApi.Data.SurrogateKeyGenerator;

namespace wasp.WebApi.Data.Models
{
    public class DataField : SurrogateBaseEntity<string, DefaultSurrogateKeyGenerator>
    {
        [Key]
        [Column("Id", Order = 1, TypeName = "nvarchar(10)"), Required]
        public override string Id { get; set; } = null!;
        
        [Column("DataAreaId", Order = 2, TypeName = "nvarchar(10)"), Required]
        public string DataAreaId { get; set; } = null!;
        
        [Column("DataTableId", Order = 3, TypeName = "nvarchar(100)"), Required]
        public string? DataTableId { get; set; }
        
        [Column("DataItemId", Order = 4, TypeName = "nvarchar(300)")]
        public string? DataItemId { get; set; }
        
        [ForeignKey("DataAreaId")]
        public DataArea DataArea { get; set; } = null!;
        
        [ForeignKey("DataTableId")]
        public DataTable? DataTable { get; set; } = null!;
        
        [ForeignKey("DataItemId,DataTableId")]
        public DataItem? DataItem { get; set; }

        public string? FilterFrom { get; set; }
        
        [NotMapped]
        public int Ordinal { get; set; }

        public string GetSqlId(IDataArea? replaced = null)
        {
            return $"{(replaced ?? DataArea).Alias}.{DataItem.Id}";
        }
    }
}