using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseRepositories
{
    public abstract class Repository
    {
        protected readonly string connectionString;
        public delegate T ReturnItemDelegate<T>(SqlDataReader reader);
        public delegate Task<T> Execution<T>(SqlCommand command);

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<T> ExecuteSqlCommand<T>(string sqlExpression, Execution<T> executionFunction, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            using (SqlConnection connection = command.Connection)
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    T item = await executionFunction.Invoke(command);
                    await transaction.CommitAsync();
                    return item;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return default;
                }
            }
        }

        private SqlCommand CreateCommand(string sqlExpression, params SqlParameter[] parameters)
        {
            var connection = new SqlConnection(connectionString);

            var command = new SqlCommand(sqlExpression, connection);
            command.Parameters.AddRange(parameters);

            return command;
        }
    }
}
