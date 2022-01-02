namespace wasp.WebApi.Data.Mts
{
    public class MtsDataField
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Ordinal { get; set; }

        public MtsDataItemInfo? DataItemInfo { get; set; }
    }

    public class MtsDataItemInfo
    {
        public string Id { get; set; } = null!;
        public string DataTableId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? PythonId { get; set; }
    }
}