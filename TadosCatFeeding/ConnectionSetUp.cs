﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding
{
    public class ConnectionSetUp
    {
        private readonly string connectionString;

        public ConnectionSetUp(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public object ExecuteSqlQuery(
            string sqlExpression,
            string connectionString,
            SqlCommand command,
            SqlParameter[] parameters)
        {
            var connection = new SqlConnection(connectionString);

            var command = new SqlCommand(sqlExpression, connection);
            command.Parameters.AddRange(parameters);

            connection.Open();

            return command;
        }
    }
}
