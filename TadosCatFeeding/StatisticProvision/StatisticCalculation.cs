using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace TadosCatFeeding.StatisticProvision
{
    public class StatisticCalculation
    {
        private readonly string connectionString;

        public StatisticCalculation(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Dictionary<string, object>> Execute(string sqlExpression)
        {
            List<Dictionary<string, object>> info = new List<Dictionary<string, object>>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }

                        info.Add(row);
                    }
                }
                return info;
            }
        }
    }
}
