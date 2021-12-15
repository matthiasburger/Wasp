namespace wasp.WebApi.Data.Entity
{
    public abstract class Entity<TType> : IEntity<TType>
    {
        public abstract TType Id { get; set; }
    }
}