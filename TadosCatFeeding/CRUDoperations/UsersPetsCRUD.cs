using Microsoft.Data.SqlClient;
using System;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.CRUDoperations
{
    public class UsersPetsCRUD : CRUD<UserPet>
    {
        public override (bool success, string report) Create(UserPet info)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = $"INSERT INTO UsersPets (User_Id, Pet_Id) VALUES ({info.UserId}, {info.PetId});";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                try
                {
                    command.ExecuteNonQuery();
                    return (true, "Pet is shared");
                }
                catch(Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

        public override (bool success, string report) Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override (UserPet objectReturned, bool success, string report) Read(int id)
        {
            throw new NotImplementedException();
        }

        public override (bool success, string report) Update(int id, UserPet info)
        {
            throw new NotImplementedException();
        }
    }
}
