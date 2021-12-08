namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    public interface ISurrogateKeyGenerator<TType>
    {
        TType? CurrentValue { get; set; }
        
        TType GetNextKey();
    }
}