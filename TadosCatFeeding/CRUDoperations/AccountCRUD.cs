using System;
using Microsoft.Data.SqlClient;

namespace TadosCatFeeding.CRUDoperations
{
    public class AccountCRUD : CRUD<Account>
    {
        public override (bool success, string report) Create(Account info)
        {         
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = $"SELECT Login FROM Users Where Login = '{info.Login}'; ";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return (false, "The user with this email already exists");
                }
                else
                {
                    sqlExpression = $"INSERT INTO Users (Login, Password, Nickname, Role) VALUES ('{info.Login}', '{info.Password}', '{info.Nickname}', '{info.Role}');";
                    command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    return (true, $"User with the name {info.Nickname} has already been created");
                }
            }
        }

        public override (Account objectReturned, bool success, string report) Read(int id)
        {      
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlExpression = $"SELECT * FROM Users WHERE Id = '{id}';";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    return reader.Read()
                        ? (new Account { Login = reader.GetString(1), Password = reader.GetString(2), Nickname = reader.GetString(3), Role = reader.GetString(4) }, true, "Object has been found")  
                        : (null, true, "Object cannot be found");
                }
                catch(Exception ex)
                {
                    return (null, false, ex.Message);
                }
            }
        }

        public override (bool success, string report) Update(int id, Account info)
        {           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlExpression = $"UPDATE Users SET Login = '{info.Login}', Nickname = '{info.Nickname}', Password = '{info.Password}', Role = '{info.Role}' Where Id = '{id}';";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                try
                {
                    command.ExecuteNonQuery();
                    return (true, "Account is updated");
                }
                catch(Exception ex)
                {
                    return (false, ex.Message);
                } 
            }
        }

        public override (bool success, string report) Delete(int id)
        {         
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlExpression = $"DELETE FROM Users WHERE Id = '{id}';";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                try
                {
                    command.ExecuteNonQuery();
                    return (true, "Account is deleted");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }
    }
}

