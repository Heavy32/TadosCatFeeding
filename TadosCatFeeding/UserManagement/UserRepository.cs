using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.Abstractions;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.UserManagement
{
    public class UserRepository : IUserRepository
    {
        public string ConnectionString { get; set; }

        public UserRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public UserModel GetUserByLogindAndPassword(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "SELECT * FROM Users WHERE Login = @login AND Password = @password";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@login", login),
                        new SqlParameter("@password", password)
                    });

                connection.Open();
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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "INSERT INTO Users (Login, Password, Nickname, Role) VALUES (@Login, @Password, @Nickname, @Role) SELECT CAST(scope_identity() AS int);";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@Login", info.Login),
                        new SqlParameter("@Password", info.Password),
                        new SqlParameter("@Nickname", info.Nickname),
                        new SqlParameter("@Role", info.Role)
                    });

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "DELETE FROM Users WHERE Id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public UserModel Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "SELECT * FROM Users WHERE Id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
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
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(
                            new UserModel
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2),
                                Nickname = reader.GetString(3),
                                Role = reader.GetString(4)
                            });
                    }
                    return users;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Update(int id, UserModel info)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "UPDATE Users SET Login = @login, Password = @password, Nickname = @nickname, Role = @role, WHERE Id = @id";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@login", id),
                        new SqlParameter("@login", info.Login),
                        new SqlParameter("@password", info.Password),
                        new SqlParameter("@nickname", info.Nickname),
                        new SqlParameter("@role", info.Role)
                    });

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
