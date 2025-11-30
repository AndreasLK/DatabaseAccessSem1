using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DatabaseAccessSem1
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString; //Destination af database

        public SqlServerConnectionFactory(string connectionString) //INIT af klasse
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
