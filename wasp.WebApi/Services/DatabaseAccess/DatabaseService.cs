using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace wasp.WebApi.Services.DatabaseAccess
{
    public interface IDatabaseService
    {
        int ExecuteQuery(string queryString);
    }

    public class DatabaseService : IDatabaseService
    {
        public int ExecuteQuery(string queryString)
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=wasp_test;Trusted_Connection=True;MultipleActiveResultSets=true";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(queryString, connection);

            connection.Open();
            
            return command.ExecuteNonQuery();
        }
    }
}
