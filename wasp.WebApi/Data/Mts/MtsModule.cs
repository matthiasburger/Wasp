using System.Collections.Generic;

using Newtonsoft.Json;

namespace wasp.WebApi.Data.Mts
{
    public class MtsModule
    {
        [JsonProperty("data-areas")] public List<MtsDataArea> DataAreas { get; set; } = new List<MtsDataArea>();

    }
}