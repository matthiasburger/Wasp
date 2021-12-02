using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wasp.WebApi.Data.Models
{
    public class SurrogateLookup
    {
        private const string DefaultGeneratorClass = "";
        
        [Key] 
        public string DataTableId { get; set; } = null!;
        
        public string? CurrentPrimaryKey { get; set; }

        public string GeneratorClass { get; set; } = DefaultGeneratorClass;
    }
    
    public class Module
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<DataArea> DataAreas { get; set; } = new List<DataArea>();
    }

    public class DataArea
    {
        public string Id { get; set; } = null!;
        public string? ModuleId { get; set; }
        
        public Module? Module { get; set; }
        public ICollection<DataField> DataFields { get; set; } = new List<DataField>();
    }
    
    public class DataField
    {
        public string Id { get; set; } = null!;

        public string DataAreaId { get; set; } = null!;
        public string? DataTableId { get; set; }
        public string? DataItemId { get; set; }
        
        public DataArea DataArea { get; set; } = null!;
        public DataItem? DataItem { get; set; }
    }
}