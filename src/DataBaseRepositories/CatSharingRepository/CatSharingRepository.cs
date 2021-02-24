using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseRepositories.CatSharingRepository
{
    public class CatSharingRepository : Repository, ICatSharingRepository
    {

        public CatSharingRepository(string connectionString) : base(connectionString) { }

        public async Task<int> CreateAsync(CatSharingCreateInDbModel info)
            => await ExecuteSqlCommand(
                    "INSERT INTO UsersCats (User_Id, Cat_Id) VALUES (@user_id, @cat_id); SET @id=SCOPE_IDENTITY();",
                    async command => await command.ExecuteNonQueryAsync(),
                    new SqlParameter[]
                        {
                            new SqlParameter("@user_Id", info.UserId),
                            new SqlParameter("@cat_id", info.CatId),
                            new SqlParameter
                            {
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Output
                            }
                        });

        public async Task<bool> IsCatSharedWithUserAsync(int userId, int catId)
            => await ExecuteSqlCommand(
                    "SELECT User_Id, Cat_Id FROM UsersCats WHERE User_Id = @user_id AND Cat_Id = @cat_id;",
                     ReturnLink,
                     new SqlParameter[]
                     {
                         new SqlParameter("@user_Id", userId),
                         new SqlParameter("@cat_Id", catId)
                     }) != null;

        private async Task<CatSharingCreateInDbModel> ReturnLink(SqlCommand command)
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            var link = reader.Read()
                ? new CatSharingCreateInDbModel(
                    (int)reader["User_Id"],
                    (int)reader["Cat_Id"])
                : null;

            reader.Close();

            return link;
        }
    }
}
