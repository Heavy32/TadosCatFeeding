using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.StatisticProvision
{
    public class StatisticRepository : IStatisticRepository
    {
        public string ConnectionString { get; set; }

        public StatisticRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<DateTime> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
        {
            List<DateTime> info = new List<DateTime>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlExpression = $"SELECT User_Id, Pet_Id, Feed_Time FROM FeedTime WHERE User_Id = @userId AND Pet_Id = @catId AND Feed_Time BETWEEN @start AND @finish";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@userId", userId),
                        new SqlParameter("@catId", catId),
                        new SqlParameter("@start", start),
                        new SqlParameter("@finish", finish)
                    });

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        info.Add(reader.GetDateTime(2));
                    }
                }
            }
            return info;
        }

        public int Create(StatisticModel info)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //change statistics2
                string sqlExpression = "INSERT INTO Statistics2 (Name, Description, SqlExpression) VALUES (@name, @description, @sqlExpression) SELECT CAST(scope_identity() AS int);";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@name", info.Name),
                        new SqlParameter("@description", info.Description),
                        new SqlParameter("@sqlExpression", info.Description)
                    });

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public StatisticModel Get(int id)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "SELECT * FROM Statistics2 WHERE Id = @id";

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return new StatisticModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            SqlExpression = reader.GetString(3)
                        };
                    }
                }
                return null;
            }
        }

        public List<StatisticModel> GetAll()
        {
            List<StatisticModel> statistics = new List<StatisticModel>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "SELECT * FROM Statistics2";

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        statistics.Add(
                            new StatisticModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                SqlExpression = reader.GetString(3)
                            });
                    }
                }
                return statistics;
            }
        }

        public void Update(int id, StatisticModel info)
        {
            throw new NotImplementedException();
        }
    }
}
