namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    public interface ISurrogateKeyGenerator
    {
        string GetNextKey(string datatable);
    }
}