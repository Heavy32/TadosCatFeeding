using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseRepositories.CatRepository
{
    public class CatRepository : Repository, ICatRepository
    {
        public CatRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> CreateAsync(CatCreateInDbModel info)
            => await ExecuteSqlCommand(
                     $"INSERT INTO Pets (Name, Owner_Id) VALUES (@name, @owner_Id); SET @id=SCOPE_IDENTITY();",
                     async command => await command.ExecuteNonQueryAsync(),
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

        public async Task<CatInDbModel> GetAsync(int id)
            => await ExecuteSqlCommand(
                    $"SELECT Id, Name, Owner_Id FROM Pets WHERE Id = @id",
                    ReturnCat,
                    new SqlParameter("@id", id));

        private async Task<CatInDbModel> ReturnCat(SqlCommand command)
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            var cat = reader.Read()
            ? new CatInDbModel(
                (int)reader["Id"],
                (string)reader["Name"],
                (int)reader["Owner_Id"])
            : null;
            reader.Close();

            return cat;
        }
    }
}
