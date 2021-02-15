using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseManagement.StatisticProvision
{
    public class StatisticCalculation : IStatisticCalculation
    {
        private readonly string connectionString;

        public StatisticCalculation(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<StatisticResult> ExecuteAsync(string sqlExpression)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                command.Transaction = transaction;

                try
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }

                        results.Add(row);
                    }
                    await reader.CloseAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }

                return new StatisticResult(results);
            }
        }
    }
}
