using Microsoft.Data.SqlClient;
using System.Data;

namespace TadosCatFeeding.CatManagement
{
    public class CatRepository : Repository, ICatRepository
    {
        public CatRepository(string connectionString) : base(connectionString)
        {
        }

        public int Create(CatCreateModel info)
        {
            return (int)ExecuteWithOutResult(
                 $"INSERT INTO Pets (Name, Owner_Id) VALUES (@name, @owner_Id); SET @id=SCOPE_IDENTITY();",
                 new SqlParameter[]
                    {
                        new SqlParameter("@name", info.Name),
                        new SqlParameter("@owner_Id", info.OwnerId),
                        new SqlParameter
                        {
                            ParameterName = "@id",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        }
                    });
        }

        public CatModel Get(int id)
        {
            return ReturnCustomItem(
                $"SELECT Id, Name, Owner_Id FROM Pets WHERE Id = @id",
                ReturnCat,
                new SqlParameter("@id", id));
        }

        private CatModel ReturnCat(SqlDataReader reader)
        {
            var cat = reader.Read()
            ? new CatModel(
                (int)reader["Id"],
                (string)reader["Name"],
                (int)reader["Owner_Id"])
            : null;
            reader.Close();
            return cat;
        }
    }
}
