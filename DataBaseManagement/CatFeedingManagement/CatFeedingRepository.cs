using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DataBaseManagement.CatFeedingManagement
{
    public class CatFeedingRepository : Repository, ICatFeedingRepository
    {
        public CatFeedingRepository(string connectionString) : base(connectionString) { }

        public async Task<int> CreateAsync(CatFeedingCreateInDbModel info)
        {
            return (int)await ExecuteWithOutResultAsync(
                "INSERT INTO FeedTime (User_Id, Pet_Id, Feed_Time) VALUES (@user_Id, @pet_Id, @feed_Time); SET @id=SCOPE_IDENTITY();",
                 new SqlParameter[]
                 {
                     new SqlParameter("@user_Id", info.UserId),
                     new SqlParameter("@pet_Id", info.CatId),
                     new SqlParameter("@feed_Time", info.FeedingTime),
                     new SqlParameter
                        {
                            ParameterName = "@id",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        }
                 });
        }

        public async Task<List<DateTime>> GetFeedingForPeriodAsync(int userId, int catId, DateTime start, DateTime finish)
        {
            List<DateTime> info = new List<DateTime>();

            List<CatFeedingInDbModel> feeds = await ReturnListCustomItemsAsync(
                "SELECT User_Id, Pet_Id, Feed_Time FROM FeedTime WHERE User_Id = @userId AND Pet_Id = @catId AND Feed_Time BETWEEN @start AND @finish",
                ReturnFeeding,
                new SqlParameter[]
                    {
                        new SqlParameter("@userId", userId),
                        new SqlParameter("@catId", catId),
                        new SqlParameter("@start", start),
                        new SqlParameter("@finish", finish)
                    });

            for (int i = 0; i < feeds.Count; i++)
            {
                info.Add(feeds[i].FeedingTime);
            }

            return info;
        }

        private CatFeedingInDbModel ReturnFeeding(SqlDataReader reader)
        {
            if (reader.Read())
            {
                return new CatFeedingInDbModel(
                    (int)reader["Id"],
                    (int)reader["User_Id"],
                    (int)reader["Owner_Id"],
                    (DateTime)reader["Feed_Time"]
                    );
            }
            reader.Close();
            return null;
        }
    }
}
