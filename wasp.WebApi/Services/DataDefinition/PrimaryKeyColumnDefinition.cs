namespace wasp.WebApi.Services.DataDefinition
{
    public class PrimaryKeyColumnDefinition : ColumnDefinition
    {
        public new bool IsNullable => false;

        public int? IdentitySeed { get; set; }
        
        public int? IdentityIncrement { get; set; }
    }
}
