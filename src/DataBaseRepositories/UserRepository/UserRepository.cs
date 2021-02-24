using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseRepositories.UserManagement
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        public async Task<UserInDbModel> GetUserByLoginAsync(string login)
            => await ExecuteSqlCommand(
                    "SELECT Id, Login, Nickname, Role, Salt, HashedPassword FROM Users WHERE Login = @login",
                    ReturnUser,
                    new SqlParameter[]
                    {
                        new SqlParameter("@login", login),
                    });      

        public async Task<int> CreateAsync(UserInDbModel info)
            => await ExecuteSqlCommand(
                    "INSERT INTO Users (Login, Nickname, Role, Salt, HashedPassword) VALUES (@Login, @Nickname, @Role, @Salt, @Password); SELECT SCOPE_IDENTITY();",
                    async command => Convert.ToInt32(await command.ExecuteScalarAsync()),
                    new SqlParameter[]
                        {
                            new SqlParameter("@Login", info.Login),
                            new SqlParameter("@Nickname", info.Nickname),
                            new SqlParameter("@Role", info.Role),
                            new SqlParameter("@Salt", info.Salt),
                            new SqlParameter("@password", info.HashedPassword),
                            new SqlParameter
                            {
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Output
                            }
                        });

        public async Task DeleteAsync(int id)
            => await ExecuteSqlCommand(
                    "DELETE FROM Users WHERE Id = @id",
                    async command => await command.ExecuteNonQueryAsync(),
                    new SqlParameter[]
                    {
                        new SqlParameter("@id", id)
                    });


        public async Task<UserInDbModel> GetAsync(int id)
            => await ExecuteSqlCommand(
                   "SELECT Id, Login, Nickname, Role, Salt, HashedPassword FROM Users WHERE Id = @id",
                   ReturnUser,
                   new SqlParameter[]
                   {
                       new SqlParameter("@id", id)
                   });

        public async Task UpdateAsync(int id, UserInDbModel info)
            => await ExecuteSqlCommand(
                   "UPDATE Users SET Login = @login, Salt = @Salt, HashedPassword = @password, Nickname = @nickname, Role = @role WHERE Id = @id;",
                   async command => await command.ExecuteNonQueryAsync(),
                   new SqlParameter[]
                       {
                           new SqlParameter("@id", id),
                           new SqlParameter("@Login", info.Login),
                           new SqlParameter("@Nickname", info.Nickname),
                           new SqlParameter("@Role", info.Role),
                           new SqlParameter("@Salt", info.Salt),
                           new SqlParameter("@password", info.HashedPassword),
                       });

        private async Task<UserInDbModel> ReturnUser(SqlCommand command)
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            UserInDbModel user = reader.Read()
                ? new UserInDbModel(
                    (int)reader["Id"],
                    (string)reader["Login"],
                    (string)reader["Nickname"],
                    (int)reader["Role"],
                    (string)reader["Salt"],
                    (string)reader["HashedPassword"])
                : null;

            reader.Close();
            return user;
        }
    }
}
