using System.Data.SqlClient;

using Microsoft.Extensions.Configuration;

namespace wasp.WebApi.Services.DatabaseAccess
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfiguration _configuration;
        
        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public int ExecuteQuery(string queryString)
        {
            string connectionString = _configuration.GetConnectionString("WaspSqlServerConnectionString");

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(queryString, connection);

            connection.Open();
            
            return command.ExecuteNonQuery();
        }
    }
}
