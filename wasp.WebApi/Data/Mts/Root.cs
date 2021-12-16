namespace wasp.WebApi.Data.Mts
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class Root
    {
        public MtsModule Module { get; set; }
    }

}