using System.Collections.Generic;

using Newtonsoft.Json;
using wasp.WebApi.Data.Models;

namespace wasp.WebApi.Data.Mts
{
    public class MtsDataArea
    {
        [JsonProperty("records")] 
        public List<MtsRecord> Records { get; set; } = new();
        
        [JsonProperty("append")] 
        public bool Append { get; set; }
        
        public MtsDataAreaInfo DataAreaInfo { get; set; }
        
        
        public override string ToString()
        {
            return $"MtsDataArea <{DataAreaInfo}>";
        }
    }

    public class MtsDataAreaInfo
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string DataTableId { get; set; }
        public string? ModuleId { get; set; }

        public override string ToString()
        {
            return $"MtsDataAreaInfo <{Id}:{Name}> on DataTable {DataTableId} for Module {ModuleId}";
        }
    }
}