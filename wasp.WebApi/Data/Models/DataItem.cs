using System.Diagnostics.CodeAnalysis;

namespace wasp.WebApi.Data.Models
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Needs implementation")]
    public class DataItem
    {

        public string DataItemId { get; set; }
        public string PythonId { get; set; }

        public object Value { get; set; }
    }
}