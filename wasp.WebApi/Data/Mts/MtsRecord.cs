using System.Collections.Generic;

using Newtonsoft.Json;
using wasp.WebApi.Data.Models;

namespace wasp.WebApi.Data.Mts
{
    public class MtsRecord
    {
        [JsonProperty("data-fields")] public List<MtsDataField> DataFields { get; set; } = new();

        [JsonProperty("data-areas")] public List<MtsDataArea> DataAreas { get; set; } = new();

        public string? DataTableId { get; set; }
        public bool UnsavedChanges { get; set; }
        public bool NewRecord { get; set; }

        public override string ToString()
        {
            return $"MtsRecord <{DataTableId}>";
        }
    }
}