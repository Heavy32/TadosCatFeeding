using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        [HttpGet("daysAndWeekendsFeedDifference")]        
        public IActionResult GetDifferenceBetweenWeekendsAndWeekDays()
        {
            return Ok("test1");
            //string sqlExpression = "Select Pet_Id, " +
            //    "CONVERT(decimal(4, 1), " +
            //    "ROUND(COUNT(CASE WHEN DATEPART(W, Feed_Time) = 6 OR DATEPART(W, Feed_Time) = 7 THEN 0 END) / 2. , 2)) " +
            //    "- CONVERT(decimal(4, 1), ROUND(COUNT(CASE WHEN DATEPART(W, Feed_Time) != 6 AND DATEPART(W, Feed_Time) != 7 THEN 0 END) / 5. , 2)) as Diff " +
            //    "FROM FeedTime " +
            //    "GROUP BY Pet_Id;";


            //List<object> info = new List<object>(); 
            //using (SqlConnection connection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("CatFeedingDB").Value))
            //{
            //    connection.Open();

            //    SqlCommand command = new SqlCommand(sqlExpression, connection);
            //    SqlDataReader reader = command.ExecuteReader();

            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            info.Add(new { PetId = reader.GetInt32(0), Diff = reader.GetDecimal(1) });
            //        }
            //    }
            //}

            //return Ok(info);
        }

        [HttpGet("feedTimeInPeriod")]
        public IActionResult GetFeedingForPeriod()
        {
            return Ok("test2");

            //List<object> info = new List<object>();

            //using (SqlConnection connection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("CatFeedingDB").Value))
            //{
            //    connection.Open();

            //    string sqlExpression = $"SELECT User_Id, Pet_Id, Feed_Time FROM FeedTime WHERE Feed_Time BETWEEN '' AND ''";
            //    SqlCommand command = new SqlCommand(sqlExpression, connection);
            //    SqlDataReader reader = command.ExecuteReader();

            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            info.Add(new
            //            {
            //                UserId = reader.GetInt32(0),
            //                PetId = reader.GetInt32(1),
            //                FeedTime = reader.GetDateTime(2)
            //            });
            //        }
            //    }
            //}
            //return Ok(info);
        }

        private IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }
    }
}
