namespace wasp.WebApi.Data.Models
{
    public class DataItem
    {

        public string DataItemId { get; set; }
        public string PythonId { get; set; }

        public object Value { get; set; }
    }
}