using System.Collections.Generic;

using Newtonsoft.Json;

namespace wasp.WebApi.Data.Mts
{
    public class MtsDataArea
    {
        [JsonProperty("records")] public List<MtsRecord> Records { get; set; } = new List<MtsRecord>();
    }
}