
using Python.Runtime;

namespace wasp.WebApi.Services.DataDefinition
{
    public class PrimaryKeyColumnDefinition : ColumnDefinition
    {
        public PrimaryKeyColumnDefinition(PyDict dataItem) : base(dataItem)
        {
            IdentityIncrement = dataItem.HasKey("IdentityIncrement") ? dataItem["IdentityIncrement"].As<int>() : null;
            IdentitySeed = dataItem.HasKey("IdentitySeed") ? dataItem["IdentitySeed"].As<int>() : null;
        }

        public new bool IsNullable => false;

        public int? IdentitySeed { get; set; }
        public int? IdentityIncrement { get; set; }
    }
}
