using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataBaseManagement.StatisticProvision
{
    public class StatisticCalculation : IStatisticCalculation
    {
        private readonly string connectionString;

        public StatisticCalculation(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public StatisticResult Execute(string sqlExpression)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    results.Add(row);
                }

                return new StatisticResult(results);
            }
        }
    }
}
