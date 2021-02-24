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
            => await ExecuteSqlCommand(
                   "INSERT INTO Statistics (Name, Description, SqlExpression) VALUES (@name, @description, @sqlExpression); SET @id=SCOPE_IDENTITY();",
                   async command => await command.ExecuteNonQueryAsync(),
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

        public async Task<StatisticInDbModel> GetAsync(int id)
            => await ExecuteSqlCommand(
                    "SELECT Name, Description, SqlExpression FROM Statistics WHERE Id = @id",               
                    ReturnStatistic,
                    new SqlParameter[]
                    {
                        new SqlParameter("@id", id)
                    });

        public async Task<List<StatisticInDbModel>> GetAllAsync()
            => await ExecuteSqlCommand(
                    "SELECT Name, Description, SqlExpression FROM Statistics",
                    ReturnListStatistic,
                    Array.Empty<SqlParameter>());

        private async Task<StatisticInDbModel> ReturnStatistic(SqlCommand command)
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

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

        private async Task<List<StatisticInDbModel>> ReturnListStatistic(SqlCommand command)
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            var list = new List<StatisticInDbModel>();
            while(await reader.ReadAsync())
                list.Add(new StatisticInDbModel(
                            (int)reader["Id"],
                            (string)reader["Name"],
                            (string)reader["Description"],
                            (string)reader["SqlExpression"]));

            reader.Close();
            return list;
        }
    }
}
