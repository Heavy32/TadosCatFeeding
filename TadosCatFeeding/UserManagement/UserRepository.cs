using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.UserManagement
{
    public class UserRepository : Repository
    {
        private readonly ConnectionSetUp connectionSetUp;

        public UserRepository(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
            connectionSetUp = new ConnectionSetUp(connectionString);
        }

        public UserModel GetUserByLogindAndPassword(string login, string password)
        {
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "SELECT * FROM Users WHERE Login = @login AND Password = @password", 
                new SqlParameter[]
                {
                    new SqlParameter("@login", login),
                    new SqlParameter("@password", password)
                });

            using (command.Connection)
            {
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();

                return reader.HasRows
                    ? new UserModel
                    {
                        Id = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        Nickname = reader.GetString(3),
                        Role = reader.GetString(4)
                    }
                    : null;
            }
        }        

        public int Create(UserModel info)
        {
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "INSERT INTO Users (Login, Password, Nickname, Role) VALUES (@Login, @Password, @Nickname, @Role) SELECT CAST(scope_identity() AS int);",
                new SqlParameter[]
                    {
                        new SqlParameter("@Login", info.Login),
                        new SqlParameter("@Password", info.Password),
                        new SqlParameter("@Nickname", info.Nickname),
                        new SqlParameter("@Role", info.Role)
                    });

            using (command.Connection)
            {
                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "DELETE FROM Users WHERE Id = @id",
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });

            using(command.Connection)
            {
                command.ExecuteNonQuery();
            }
        }

        public UserModel Get(int id)
        {
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "SELECT * FROM Users WHERE Id = @id",
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });

            using (command.Connection)
            {
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();

                return reader.HasRows
                    ? new UserModel
                    {
                        Id = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        Nickname = reader.GetString(3),
                        Role = reader.GetString(4)
                    }
                    : null;
            }
        }

        public List<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, UserModel info)
        {
            SqlCommand command = connectionSetUp.ExecuteSqlQuery(
                "UPDATE Users SET Login = @login, Password = @password, Nickname = @nickname, Role = @role WHERE Id = @id;",
                new SqlParameter[]
                    {
                        new SqlParameter("@id", id),
                        new SqlParameter("@login", info.Login),
                        new SqlParameter("@password", info.Password),
                        new SqlParameter("@nickname", info.Nickname),
                        new SqlParameter("@role", info.Role)
                    });

            using (command.Connection)
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
