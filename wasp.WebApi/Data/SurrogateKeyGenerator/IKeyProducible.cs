using System.Threading.Tasks;

namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    internal interface IKeyProducible
    {
        Task SetKey();
    }
}