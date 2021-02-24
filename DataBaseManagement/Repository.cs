using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseManagement
{
    public abstract class Repository
    {
        protected readonly string connectionString;
        public delegate T ReturnItemDelegate<T>(SqlDataReader reader);

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected async Task<T> ReturnCustomItemAsync<T>(string sqlExpression, ReturnItemDelegate<T> function, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            T item = default;
            using (SqlConnection connection = command.Connection)
            {
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    item = function.Invoke(await command.ExecuteReaderAsync());
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }

            }
            return item;
        }

        protected async Task<List<T>> ReturnListCustomItemsAsync<T>(string sqlExpression, ReturnItemDelegate<T> function, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);
            List<T> items = new List<T>();
            using (SqlConnection connection = command.Connection)
            {
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        items.Add(function.Invoke(reader));
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return items;
        }

        protected async Task ExecuteAsync(string sqlExpression, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            using (SqlConnection connection = command.Connection)
            {
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
        }

        protected async Task<object> ExecuteWithOutResultAsync(string sqlExpression, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            object result = default;
            using (SqlConnection connection = command.Connection)
            {
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    await command.ExecuteNonQueryAsync();
                    SqlParameter outParameter = parameters.FirstOrDefault(x => x.Direction == ParameterDirection.Output);
                    result = outParameter.Value;
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
            return result;
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
