using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Extensions.Configuration;

namespace wasp.WebApi.Services.DatabaseAccess
{
    public class DatabaseService : IDatabaseService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private SqlConnection Connection { get; set; }
        private bool _disposed;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            InitSqlConnection(_configuration.GetConnectionString("WaspSqlServerConnectionString"));

        }
        
        protected void InitSqlConnection(string connectionString, bool byName = false)
        {
            Connection = new SqlConnection(connectionString);
        }
        
        public int ExecuteQuery(string queryString)
        {
            string connectionString = _configuration.GetConnectionString("WaspSqlServerConnectionString");

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(queryString, connection);

            connection.Open();
            
            return command.ExecuteNonQuery();
        }

        public SqlCommand CreateSqlCmd(string sqlQuery)
        {
            return new SqlCommand(sqlQuery, Connection);
        }
        
        protected void OpenConnection()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }


        public Microsoft.Data.SqlClient.SqlConnection GetNewConnection()
        {
            string connectionString = _configuration.GetConnectionString("WaspSqlServerConnectionString");
            return new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose sql connection
                    // CloseConnection();


                    try
                    {
                        Connection?.Dispose();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                _disposed = true;
            }
        }
        

        /// <summary>
        /// Dispose the SqlConnection
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
