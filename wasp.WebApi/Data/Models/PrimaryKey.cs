using System.ComponentModel.DataAnnotations;

namespace wasp.WebApi.Data.Models
{
    public class PrimaryKey
    {
        [Key] 
        public string TableName { get; set; } = null!;
        
        public string CurrentId { get; set; } = null!;
    }
}