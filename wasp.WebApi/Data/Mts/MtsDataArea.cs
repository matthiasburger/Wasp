using System.Collections.Generic;

using Newtonsoft.Json;

namespace wasp.WebApi.Data.Mts
{
    public class MtsDataArea
    {
        [JsonProperty("records")] 
        public List<MtsRecord> Records { get; set; } = new List<MtsRecord>();

        public MtsDataAreaInfo DataAreaInfo { get; set; }
    }

    public class MtsDataAreaInfo
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string DataTableId { get; set; }
        public string? ModuleId { get; set; }
    }
}