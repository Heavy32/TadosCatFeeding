using Microsoft.Data.SqlClient;
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

        public SqlCommand SetUp(string sqlExpression, SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.Parameters.AddRange(parameters);

            connection.Open();

            return command;
        }
    }
}
