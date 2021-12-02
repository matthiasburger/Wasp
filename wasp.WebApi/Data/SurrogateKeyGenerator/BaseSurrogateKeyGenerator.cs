namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    public abstract class BaseSurrogateKeyGenerator : ISurrogateKeyGenerator
    {
        public abstract string GetNextKey(string datatable);
    }
}