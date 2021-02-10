using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        protected T ReturnCustomItem<T>(string sqlExpression, ReturnItemDelegate<T> function, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            T item = default;
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    item = function.Invoke(command.ExecuteReader());
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

            }
            return item;
        }

        protected List<T> ReturnListCustomItems<T>(string sqlExpression, ReturnItemDelegate<T> function, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);
            List<T> items = new List<T>();
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(function.Invoke(reader));
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return items;
        }

        protected void Execute(string sqlExpression, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        protected object ExecuteWithOutResult(string sqlExpression, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateCommand(sqlExpression, parameters);

            object result = default;
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;
                try
                {
                    command.ExecuteNonQuery();
                    SqlParameter outParameter = parameters.FirstOrDefault(x => x.Direction == ParameterDirection.Output);
                    result = outParameter.Value;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
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
