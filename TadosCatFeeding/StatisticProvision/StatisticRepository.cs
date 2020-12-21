using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.StatisticProvision
{
    public class StatisticRepository : Repository
    {
        public string ConnectionString { get; set; }
        private readonly ConnectionSetUp connectionSetUp;

        public StatisticRepository(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
            connectionSetUp = new ConnectionSetUp(connectionString);
        }

        public List<DateTime> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
        {
            List<DateTime> info = new List<DateTime>();

            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "SELECT User_Id, Pet_Id, Feed_Time FROM FeedTime WHERE User_Id = @userId AND Pet_Id = @catId AND Feed_Time BETWEEN @start AND @finish",
                new SqlParameter[]
                    {
                        new SqlParameter("@userId", userId),
                        new SqlParameter("@catId", catId),
                        new SqlParameter("@start", start),
                        new SqlParameter("@finish", finish)
                    });

            using (command.Connection)
            {        
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
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "INSERT INTO Statistics (Name, Description, SqlExpression) VALUES (@name, @description, @sqlExpression) SELECT CAST(scope_identity() AS int);",
                new SqlParameter[]
                    {
                        new SqlParameter("@name", info.Name),
                        new SqlParameter("@description", info.Description),
                        new SqlParameter("@sqlExpression", info.Description)
                    });

            using (command.Connection)
            {         
                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public StatisticModel Get(int id)
        {
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "SELECT * FROM Statistics WHERE Id = @id",
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });

            using (command.Connection)
            {
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
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "SELECT * FROM Statistics",
                new SqlParameter[] { });

            using (command.Connection)
            {                
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
