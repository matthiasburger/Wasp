using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wasp.WebApi.Data.Entity;

namespace wasp.WebApi.Data.Models
{
    public class PrimaryKey : IEntity<string>
    {
        [Key] 
        public string TableName { get; set; } = null!;
        
        public string CurrentId { get; set; } = null!;
        
        [NotMapped]
        public string Id
        {
            get => TableName;
            set => TableName = value;
        }
    }
}