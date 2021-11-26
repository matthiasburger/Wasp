using Microsoft.Data.SqlClient;

namespace wasp.WebApi.Services.DatabaseAccess
{
    public interface IDatabaseService
    {
        int ExecuteQuery(string queryString);

        SqlConnection GetNewConnection();

        System.Data.SqlClient.SqlCommand CreateSqlCmd(string sqlQuery);
    }
}