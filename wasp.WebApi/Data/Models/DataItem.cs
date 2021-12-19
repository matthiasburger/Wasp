using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using wasp.WebApi.Data.Mts;

namespace wasp.WebApi.Data.Models
{
    [Table("DataItem")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Used by EF")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Set-Accessor is accessed by EF")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global", Justification = "Foreign-Key-Lists by EF")]
    public class DataItem
    {
        [Column("Id", Order = 0, TypeName = "nvarchar(300)"), Required]
        // [Key]
        public string Id { get; set; } = null!;

        [Column("DataTableId", Order = 1, TypeName = "nvarchar(100)"), Required]
        // [Key]
        public string DataTableId { get; set; } = null!;
        
        [Column("Name", Order = 2, TypeName = "nvarchar(400)"), Required]
        public string Name { get; set; } = null!;

        [Column("PythonId", Order = 3, TypeName = "nvarchar(100)")]
        public string? PythonId { get; set; }


        [ForeignKey("DataTableId")] 
        public DataTable DataTable { get; set; } = null!;

        public ICollection<Relation> KeyRelations { get; set; } = new List<Relation>();
        public ICollection<Relation> ReferenceRelations { get; set; } = new List<Relation>();
        
        public ICollection<DataAreaReference> KeyDataAreaReference { get; set; } = new List<DataAreaReference>();
        public ICollection<DataAreaReference> ReferenceDataAreaReference { get; set; } = new List<DataAreaReference>();

        
        public string GetSqlId() => $"{DataTable.Alias}.{Id}";
        
        public MtsDataItemInfo GetDataItemInfo ()
        {
            return new MtsDataItemInfo
            {
                Id = Id,
                DataTableId = DataTableId,
                Name = Name,
                PythonId = PythonId
            };
        }
    }
}