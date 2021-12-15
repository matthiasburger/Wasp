using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wasp.WebApi.Data.Entity;

namespace wasp.WebApi.Data.Models
{
    public class DataAreaReference : IEntity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", Order = 0), Required]
        public long Id { get; set; }

        [Column("DataAreaId", Order = 1, TypeName = "nvarchar(10)"), Required]
        public string DataAreaId { get; set; }

        [Column("ReferenceDataItemId", Order = 2, TypeName = "nvarchar(300)"), Required]
        public string ReferenceDataItemId { get; set; } = null!;

        [Column("ReferenceDataTableId", Order = 3, TypeName = "nvarchar(100)"), Required]
        public string ReferenceDataTableId { get; set; } = null!;
        
        [Column("KeyDataItemId", Order = 4, TypeName = "nvarchar(300)"), Required]
        public string KeyDataItemId { get; set; } = null!;

        [Column("KeyDataItemDataTableId", Order = 5, TypeName = "nvarchar(100)"), Required]
        public string KeyDataItemDataTableId { get; set; } = null!;
        
        
        [ForeignKey("ReferenceDataItemId, ReferenceDataTableId")]
        public DataItem ReferenceDataItem { get; set; }

        [ForeignKey("KeyDataItemId, KeyDataItemDataTableId")]
        public DataItem KeyDataItem { get; set; }
        
        [ForeignKey("DataAreaId")]
        public DataArea DataArea { get; set; }
    }
}