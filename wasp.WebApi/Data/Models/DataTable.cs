using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace wasp.WebApi.Data.Models
{
    [Table("DataTable")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Used by EF")]
    public class DataTable
    {
        [Column("Id", Order = 0, TypeName = "nvarchar(100)"), Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = null!;

        [Column("Name", Order = 1, TypeName = "nvarchar(200)"), Required]
        public string Name { get; set; } = null!;

        [Column("SqlId", Order = 2, TypeName = "nvarchar(200)"), Required]
        public string SqlId { get; set; } = null!;

        public ICollection<DataItem> DataItems { get; set; } = new List<DataItem>();
    }
}