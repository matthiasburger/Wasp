namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    public abstract class BaseSurrogateKeyGenerator<TType> : ISurrogateKeyGenerator<TType>
    {
        public TType? CurrentValue { get; set; }
        public abstract TType GetNextKey();
    }
}