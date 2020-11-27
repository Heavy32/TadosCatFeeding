using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.CatSharingManagement
{
    public class CatSharingRepository : ICatSharingRepository
    {
        public string ConnectionString { get; set; }

        public CatSharingRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int Create(CatSharingModel info)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "INSERT INTO UsersPets (User_Id, Pet_Id) VALUES (@user_id, @pet_id) SELECT CAST(scope_identity() AS int);";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", info.UserId),
                        new SqlParameter("@cat_Id", info.CatId)
                    });

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CatSharingModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<CatSharingModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, CatSharingModel info)
        {
            throw new NotImplementedException();
        }

        //add to interface
        public bool IsPetSharedWithUser(int userId, int petId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlExpression = "SELECT * FROM UsersPets WHERE User_Id = @user_id AND Pet_Id = @pet_id;";

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.AddRange(
                    new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", userId),
                        new SqlParameter("@pet_Id", petId)
                    });

                connection.Open();

                return command.ExecuteReader().HasRows;
            }
        }
    }
}
