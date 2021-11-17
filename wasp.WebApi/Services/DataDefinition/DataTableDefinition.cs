namespace wasp.WebApi.Services.DataDefinition
{
    public class DataTableDefinition
    {
        public string TableName { get; set; }
        public PrimaryKeyColumnDefinition[] PrimaryKeyColumns { get; set; }
    }
}
