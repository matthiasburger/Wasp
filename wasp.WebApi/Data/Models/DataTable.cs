using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wasp.WebApi.Data.Models
{
    [Table("DataTable")]
    public class DataTable
    {
        [Column("Id", Order = 0, TypeName = "nvarchar(100)")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Column("Name", Order = 1, TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Column("SqlId", Order = 2, TypeName = "nvarchar(200)")]
        public string SqlId { get; set; }
    }
}