using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseManagement.UserManagement
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        public async Task<UserInDbModel> GetUserByLoginAsync(string login)
        {
            return await ReturnCustomItemAsync(
                "SELECT Id, Login, Nickname, Role, Salt, HashedPassword FROM Users WHERE Login = @login",
                ReturnUser,
                new SqlParameter[]
                {
                    new SqlParameter("@login", login),
                });
        }        

        public async Task<int> CreateAsync(UserInDbModel info)
        {            
            int id = (int)await ExecuteWithOutResultAsync(
                "INSERT INTO Users (Login, Nickname, Role, Salt, HashedPassword) VALUES (@Login, @Nickname, @Role, @Salt, @Password); SET @id=SCOPE_IDENTITY();",
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

            return id;
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteAsync(
                "DELETE FROM Users WHERE Id = @id",
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public async Task<UserInDbModel> GetAsync(int id)
        {
            return await ReturnCustomItemAsync(
                "SELECT Id, Login, Nickname, Role, Salt, HashedPassword FROM Users WHERE Id = @id",
                ReturnUser,
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public async Task UpdateAsync(int id, UserInDbModel info)
        {
            await ExecuteAsync(
                "UPDATE Users SET Login = @login, Salt = @Salt, HashedPassword = @password, Nickname = @nickname, Role = @role WHERE Id = @id;",
                new SqlParameter[]
                    {
                        new SqlParameter("@id", id),
                        new SqlParameter("@Login", info.Login),
                        new SqlParameter("@Nickname", info.Nickname),
                        new SqlParameter("@Role", info.Role),
                        new SqlParameter("@Salt", info.Salt),
                        new SqlParameter("@password", info.HashedPassword),
                    });
        }

        private UserInDbModel ReturnUser(SqlDataReader reader)
        {
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
