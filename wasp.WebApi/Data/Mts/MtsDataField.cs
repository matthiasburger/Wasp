using System;

namespace wasp.WebApi.Data.Mts
{
    public class MtsDataField
    {
        public string Name { get; set; }
        [Obsolete("Should be object?")]
        public string? Value { get; set; }
        public int Ordinal { get; set; }

        public MtsDataItemInfo? DataItemInfo { get; set; }
        
        
        public override string ToString()
        {
            return $"MtsDataField <{Name}> of DataItem <{DataItemInfo})>: value = {Value}";
        }
    }

    public class MtsDataItemInfo
    {
        public string Id { get; set; } = null!;
        public string DataTableId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? PythonId { get; set; }
        public bool Required { get; set; }

        public override string ToString()
        {
            return $"MtsDataItemInfo <{Id}:{DataTableId}:{Name}>{(Required ? "*":"")}";
        }
    }
}