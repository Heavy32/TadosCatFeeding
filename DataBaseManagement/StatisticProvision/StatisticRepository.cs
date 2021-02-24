using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseManagement.StatisticProvision
{
    public class StatisticRepository : Repository, IStatisticRepository
    {
        public StatisticRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> CreateAsync(StatisticInDbModel info)
        {
            return (int) await ExecuteWithOutResultAsync(
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

        public async Task<StatisticInDbModel> GetAsync(int id)
        {
            return await ReturnCustomItemAsync(
                "SELECT Name, Description, SqlExpression FROM Statistics WHERE Id = @id",
                ReturnStatistic,
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public async Task<List<StatisticInDbModel>> GetAllAsync()
        {
            return await ReturnListCustomItemsAsync(
                "SELECT Name, Description, SqlExpression FROM Statistics",
                ReturnStatistic,
                Array.Empty<SqlParameter>());
        }

        private StatisticInDbModel ReturnStatistic(SqlDataReader reader)
        {
            StatisticInDbModel statistic = reader.Read()
                ? new StatisticInDbModel(
                    (int)reader["Id"],
                    (string)reader["Name"],
                    (string)reader["Description"],
                    (string)reader["SqlExpression"])
                : null;

            reader.Close();
            return statistic;
        }
    }
}
