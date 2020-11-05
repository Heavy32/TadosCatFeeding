using Microsoft.Data.SqlClient;
using System;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.CRUDoperations
{
    public class PetCRUD : CRUD<Pet>
    {
        public override (bool success, string report) Create(Pet petInfo)
        {           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                           
                try
                {
                    string sqlExpression = $"INSERT INTO Pets (Name, Owner_Id) VALUES ('{petInfo.Name}', '{petInfo.OwnerId}');";
                    SqlCommand command = new SqlCommand(sqlExpression, connection, transaction);

                    command.ExecuteNonQuery();

                    sqlExpression = $"SELECT Id FROM Pets WHERE Name = '{petInfo.Name}' AND Owner_Id = {petInfo.OwnerId}";
                    command.CommandText = sqlExpression;
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    int petId = reader.GetInt32(0);
                    reader.Close();

                    sqlExpression = $"INSERT INTO UsersPets (User_Id, Pet_Id) VALUES ('{petInfo.OwnerId}', '{petId}')";
                    command.CommandText = sqlExpression;
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    return (true, "Pet is added");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return (false, ex.Message);
                }
            }
        }

        public override (bool success, string report) Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override (Pet objectReturned, bool success, string report) Read(int id)
        {
            throw new NotImplementedException();
        }

        public override (bool success, string report) Update(int id, Pet info)
        {
            throw new NotImplementedException();
        }
    }
}
