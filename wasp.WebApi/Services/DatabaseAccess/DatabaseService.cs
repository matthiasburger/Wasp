using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;

namespace wasp.WebApi.Services.DatabaseAccess
{
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global", Justification = "Dependency-Injected")]
    public class DatabaseService : IDatabaseService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private SqlConnection? Connection { get; set; }
        private bool _disposed;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _initSqlConnection(_configuration.GetConnectionString("WaspSqlServerConnectionString"));

        }

        private void _initSqlConnection(string connectionString)
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
            if (Connection is null)
                throw new IncompleteInitialization();
            
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CloseConnection()
        {
            if (Connection is null)
                throw new IncompleteInitialization();
            
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
            if (_disposed) 
                return;
            
            if (disposing)
            {
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
        

        /// <summary>
        /// Dispose the SqlConnection
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
