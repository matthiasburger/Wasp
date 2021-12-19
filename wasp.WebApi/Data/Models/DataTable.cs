using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using wasp.WebApi.Data.Entity;

namespace wasp.WebApi.Data.Models
{
    [Table("DataTable")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Used by EF")]
    public class DataTable : IEntity<string>
    {
        [Column("Id", Order = 0, TypeName = "nvarchar(100)"), Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = null!;

        [Column("Name", Order = 1, TypeName = "nvarchar(200)"), Required]
        public string Name { get; set; } = null!;

        [Column("SqlId", Order = 2, TypeName = "nvarchar(200)"), Required]
        public string SqlId { get; set; } = null!;

        public ICollection<DataItem> DataItems { get; set; } = new List<DataItem>();
        public ICollection<DataArea> DataAreas { get; set; } = new List<DataArea>();
        
        [NotMapped]
        public string? Alias { get; set; }

        
        public string GetSqlId()
        {
            if (Alias is null)
                return SqlId;
            
            return $"{SqlId} as {Alias}";
        }
    }
}