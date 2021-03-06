﻿using System;
using System.Data;
using System.Data.SqlClient;
using Scrap.Core;

namespace Scrap.Domain
{
    public class MsSqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public MsSqlConnectionFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string is null or empty", "connectionString");
            _connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
