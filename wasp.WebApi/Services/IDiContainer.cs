namespace wasp.WebApi.Services
{
    public interface IDiContainer
    {
        object Resolve(string name);
    }
}