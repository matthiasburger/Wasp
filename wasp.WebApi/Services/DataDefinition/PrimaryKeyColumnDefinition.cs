namespace wasp.WebApi.Services.DataDefinition
{
    public class PrimaryKeyColumnDefinition : ColumnDefinition
    {
        public int? IdentitySeed { get; set; }
        
        public int? IdentityIncrement { get; set; }
    }
}
