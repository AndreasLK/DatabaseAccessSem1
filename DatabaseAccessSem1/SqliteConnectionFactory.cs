using System;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Data.Sqlite;

namespace DatabaseAccessSem1
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString; //Destination af database

        public SqliteConnectionFactory(string connectionString) //INIT af klasse
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() {
            return new SqliteConnection(_connectionString);
        }
    }
}
