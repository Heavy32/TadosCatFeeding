using System;
using Microsoft.Data.SqlClient;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.CRUDoperations
{
    public class PetFeedingCRUD : CRUD<PetFeeding>
    {
        public override (bool success, string report) Create(PetFeeding info)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlExpression = $"INSERT INTO FeedTime (User_Id, Pet_Id, Feed_Time) VALUES ({info.UserId}, {info.PetId}, '{info.FeedTime}');";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                try
                {
                    command.ExecuteNonQuery();
                    return (true, "Pet is fed");
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

        public override (PetFeeding objectReturned, bool success, string report) Read(int id)
        {
            throw new NotImplementedException();
        }

        public override (bool success, string report) Update(int id, PetFeeding info)
        {
            throw new NotImplementedException();
        }
    }
}
