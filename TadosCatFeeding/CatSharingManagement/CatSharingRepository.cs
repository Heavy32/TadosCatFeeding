using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TadosCatFeeding.CatSharingManagement
{
    public class CatSharingRepository : ICatSharingRepository
    {
        public string ConnectionString { get; set; }
        private readonly ConnectionSetUp connectionSetUp;

        public CatSharingRepository(string connectionString)
        {
            ConnectionString = connectionString;
            connectionSetUp = new ConnectionSetUp(connectionString);
        }

        public int Create(CatSharingModel info)
        {
            SqlCommand command = connectionSetUp.SetUp(
                "INSERT INTO UsersPets (User_Id, Pet_Id) VALUES (@user_id, @pet_id) SELECT CAST(scope_identity() AS int);",
                new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", info.UserId),
                        new SqlParameter("@pet_id", info.CatId)
                    });

            using (command.Connection)
            {
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

        public bool IsPetSharedWithUser(int userId, int petId)
        {
            SqlCommand command = connectionSetUp.SetUp(
                "SELECT * FROM UsersPets WHERE User_Id = @user_id AND Pet_Id = @pet_id;",
                    new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", userId),
                        new SqlParameter("@pet_Id", petId)
                    });

            using (command.Connection)
            {                
                return command.ExecuteReader().HasRows;
            }
        }
    }
}
