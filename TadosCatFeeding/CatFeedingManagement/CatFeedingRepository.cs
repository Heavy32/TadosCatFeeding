using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.CatFeedingManagement
{
    public class CatFeedingRepository : ICatFeedingRepository
    {
        public string ConnectionString { get; set; }

        public CatFeedingRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int Create(CatFeedingModel info)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "INSERT INTO FeedTime (User_Id, Pet_Id, Feed_Time), VALUES (@user_Id, @pet_Id, @feed_Time) SELECT CAST(scope_identity() AS int);";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(
                    new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", info.UserId),
                        new SqlParameter("@pet_Id", info.CatId),
                        new SqlParameter("@feed_Time", info.FeedingTime)
                    });

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CatFeedingModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<CatFeedingModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, CatFeedingModel info)
        {
            throw new NotImplementedException();
        }
    }
}
