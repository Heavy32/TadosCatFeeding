using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace TadosCatFeeding.StatisticProvision
{
    public class StatisticRepository : Repository, IStatisticRepository
    {
        public StatisticRepository(string connectionString) : base(connectionString)
        {
        }

        public int Create(StatisticModel info)
        {
            return (int)ExecuteWithOutResult(
                "INSERT INTO Statistics (Name, Description, SqlExpression) VALUES (@name, @description, @sqlExpression); SET @id=SCOPE_IDENTITY();",
                new SqlParameter[]
                    {
                        new SqlParameter("@name", info.Name),
                        new SqlParameter("@description", info.Description),
                        new SqlParameter("@sqlExpression", info.Description),
                        new SqlParameter
                        {
                            ParameterName = "@id",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        }
                    });
        }

        public StatisticModel Get(int id)
        {
            return ReturnCustomItem(
                "SELECT Name, Description, SqlExpression FROM Statistics WHERE Id = @id",
                ReturnStatistic,
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public List<StatisticModel> GetAll()
        {
            return ReturnListCustomItems(
                "SELECT Name, Description, SqlExpression FROM Statistics",
                ReturnStatistic,
                new SqlParameter[] { });
        }

        private StatisticModel ReturnStatistic(SqlDataReader reader)
        {
            if (reader.Read())
            {
                return new StatisticModel(
                    (int)reader["Id"],
                    (string)reader["Name"],
                    (string)reader["Description"],
                    (string)reader["SqlExpression"]);
            }
            return null;
        }
    }
}
