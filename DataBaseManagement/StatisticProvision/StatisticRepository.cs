﻿using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataBaseManagement.StatisticProvision
{
    public class StatisticRepository : Repository, IStatisticRepository
    {
        public StatisticRepository(string connectionString) : base(connectionString)
        {
        }

        public int Create(StatisticInDbModel info)
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

        public StatisticInDbModel Get(int id)
        {
            return ReturnCustomItem(
                "SELECT Name, Description, SqlExpression FROM Statistics WHERE Id = @id",
                ReturnStatistic,
                new SqlParameter[]
                {
                    new SqlParameter("@id", id)
                });
        }

        public List<StatisticInDbModel> GetAll()
        {
            return ReturnListCustomItems(
                "SELECT Name, Description, SqlExpression FROM Statistics",
                ReturnStatistic,
                new SqlParameter[] { });
        }

        private StatisticInDbModel ReturnStatistic(SqlDataReader reader)
        {
            if (reader.Read())
            {
                return new StatisticInDbModel(
                    (int)reader["Id"],
                    (string)reader["Name"],
                    (string)reader["Description"],
                    (string)reader["SqlExpression"]);
            }
            return null;
        }
    }
}
