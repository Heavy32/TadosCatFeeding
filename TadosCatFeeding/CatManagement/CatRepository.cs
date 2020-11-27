using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.CatManagement
{
    public class CatRepository : ICatRepository
    {
        public string ConnectionString { get; set; }

        public CatRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int Create(CatModel info)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlExpression = $"INSERT INTO Pets (Name, Owner_Id) VALUES (@name, @owner_Id) SELECT CAST(scope_identity() AS int);";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@name", info.Name),
                        new SqlParameter("@owner_Id", info.OwnerId)
                    });

                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CatModel Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlExpression = $"SELECT * FROM Pets WHERE Id = @id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@id", id));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return new CatModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            OwnerId = reader.GetInt32(2)
                        };
                    }
                }
                return null;
            }
        }

        public List<CatModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, CatModel info)
        {
            throw new NotImplementedException();
        }
    }
}
