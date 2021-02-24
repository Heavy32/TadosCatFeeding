﻿using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseManagement.CatSharingManagement
{
    public class CatSharingRepository : Repository, ICatSharingRepository
    {

        public CatSharingRepository(string connectionString) : base(connectionString) { }

        public async Task<int> CreateAsync(CatSharingCreateInDbModel info)
        {
            return (int)await ExecuteWithOutResultAsync(
                "INSERT INTO UsersPets (User_Id, Pet_Id) VALUES (@user_id, @pet_id); SET @id=SCOPE_IDENTITY();",
                new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", info.UserId),
                        new SqlParameter("@pet_id", info.CatId),
                        new SqlParameter
                        {
                            ParameterName = "@id",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        }
                    });
        }

        public async Task<bool> IsPetSharedWithUserAsync(int userId, int petId)
        {
            CatSharingCreateInDbModel link = await ReturnCustomItemAsync(
                "SELECT User_Id, Pet_Id FROM UsersPets WHERE User_Id = @user_id AND Pet_Id = @pet_id;",
                    ReturnLink,
                    new SqlParameter[]
                    {
                        new SqlParameter("@user_Id", userId),
                        new SqlParameter("@pet_Id", petId)
                    });

            return link != null;
        }

        private CatSharingCreateInDbModel ReturnLink(SqlDataReader reader)
        {
            CatSharingCreateInDbModel link = reader.Read()
                ? new CatSharingCreateInDbModel(
                    (int)reader["User_Id"],
                    (int)reader["Pet_Id"])
                : null;

            reader.Close();

            return link;
        }
    }
}
