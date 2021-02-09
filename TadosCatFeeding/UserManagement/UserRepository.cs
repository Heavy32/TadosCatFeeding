using Microsoft.Data.SqlClient;
using System.Data;
using TadosCatFeeding.UserManagement.PasswordProtection;

namespace TadosCatFeeding.UserManagement
{
    public class UserRepository : Repository, IUserRepository
    {
        private readonly HashWithSaltProtector protection;

        public UserRepository(string connectionString, HashWithSaltProtector protection) : base(connectionString)
        {
            this.protection = protection;
        }

        public UserInDB GetUserByLogin(string login)
        {
            return ReturnCustomItem(
                "SELECT Id, Login, Nickname, Role, Salt, HashedPassword FROM Users WHERE Login = @login",
                ReturnUser,
                new SqlParameter[]
                {
                    new SqlParameter("@login", login),
                });
        }        

        public int Create(UserInDB info)
        {            
            int id = (int)ExecuteWithOutResult(
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

        public void Delete(int id)
        {
            Execute(
                "DELETE FROM Users WHERE Id = @id",
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public UserInDB Get(int id)
        {
            return ReturnCustomItem(
                "SELECT Id, Login, Nickname, Role, Salt, HashedPassword FROM Users WHERE Id = @id",
                ReturnUser,
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public void Update(int id, UserInDB info)
        {
            Execute(
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

        private UserInDB ReturnUser(SqlDataReader reader)
        {
            UserInDB user = reader.Read()
                ? new UserInDB(
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
