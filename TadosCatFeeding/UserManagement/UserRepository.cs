using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.Abstractions;
using TadosCatFeeding.CRUDoperations;

namespace TadosCatFeeding.Users
{
    public class UsersRepository : IRepository<UserModel>
    {
        public string ConnectionString { get; set; }

        public UsersRepository(string connectionString)
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

        public void Create(UserModel info)
        {
            Random rnd = new Random();
            int id = rnd.Next(Int32.MaxValue, 0);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "INSERT INTO Users (Id, Login, Password, Nickname, Role) VALUES (@Id, @Login, @Password, @Nickname, @Role);";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@Id", id),
                        new SqlParameter("@Login", info.Login),
                        new SqlParameter("@Password", info.Password),
                        new SqlParameter("@Nickname", info.Nickname),
                        new SqlParameter("@Role", info.Role)
                    });

                connection.Open();
                command.ExecuteNonQuery();
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
                string sqlExpression = "SELECT FROM Users WHERE Id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

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

        public IList<UserModel> GetAll()
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
                string sqlExpression = "UPDATE Users SET Login = @login, Password = @password, Nickname = @nickname, Role = @role";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
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
