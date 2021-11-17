using System.Threading.Tasks;

using wasp.WebApi.Services.DatabaseAccess;

namespace wasp.WebApi.Services.DataDefinition
{

    public class SqlServerDataDefinitionService : BaseDataDefinitionService
    {
        public SqlServerDataDefinitionService(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
