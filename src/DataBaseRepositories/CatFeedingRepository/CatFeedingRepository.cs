using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DataBaseRepositories.CatFeedingRepository
{
    public class CatFeedingRepository : Repository, ICatFeedingRepository
    {
        public CatFeedingRepository(string connectionString) : base(connectionString) { }

        public async Task<int> CreateAsync(CatFeedingCreateInDbModel info)
            => await ExecuteSqlCommand(
                    "INSERT INTO FeedTime (User_Id, Cat_Id, Feed_Time) VALUES (@user_Id, @cat_Id, @feed_Time);",
                    async command => await command.ExecuteNonQueryAsync(),
                    new SqlParameter[]
                     {
                         new SqlParameter("@user_Id", info.UserId),
                         new SqlParameter("@cat_Id", info.CatId),
                         new SqlParameter("@feed_Time", info.FeedingTime),
                     });
        
        public async Task<List<CatFeedingInDbModel>> GetFeedingsForPeriodAsync(int userId, int catId, DateTime start, DateTime finish)
            => await ExecuteSqlCommand(
                    "SELECT Id, User_Id, Cat_Id, Feed_Time FROM FeedTime WHERE User_Id = @userId AND Cat_Id = @catId AND Feed_Time BETWEEN @start AND @finish",
                    ReturnListFeeding,
                    new SqlParameter[]
                        {
                            new SqlParameter("@userId", userId),
                            new SqlParameter("@catId", catId),
                            new SqlParameter("@start", start),
                            new SqlParameter("@finish", finish)
                        });        

        private async Task<List<CatFeedingInDbModel>> ReturnListFeeding(SqlCommand command)
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            var list = new List<CatFeedingInDbModel>();

            while (reader.Read())
            {
                list.Add(new CatFeedingInDbModel(
                    (int)reader["Id"],
                    (int)reader["Cat_Id"],
                    (int)reader["User_Id"],
                    (DateTime)reader["Feed_Time"]));
            }
            reader.Close();
            return list;
        }
    }
}
