namespace wasp.WebApi.Data.Entity
{
    public interface IEntity<TType>
    {
        TType Id { get; set; }
    }
}