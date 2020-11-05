using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TadosCatFeeding.CRUDoperations;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PetFeedingController : ControllerBase
    {    
        [HttpPost("FeedPet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult FeedPet(int petId)
        {
            int userId = GetIdByName(User.Identity.Name);

            if (!CanUserFeedPet(userId, petId))
            {
                return BadRequest("You cannot feed this pet");
            }

            PetFeedingCRUD petFeeding = new PetFeedingCRUD();

            var petFeedingInfo = new PetFeeding { UserId = userId, PetId = petId, FeedTime = DateTime.UtcNow.ToString("s") };

            (bool success, string report) = petFeeding.Create(petFeedingInfo);

            if (success)
            {
                return Ok(report);
            }
            else
            {
                return BadRequest(report);
            }
        }

        private bool CanUserFeedPet(int userId, int petId)
        {
            string sqlExpression = $"SELECT User_Id, Pet_Id FROM UsersPets WHERE User_Id = {userId} AND Pet_Id = {petId};";
            //need to hide
            string connectionString = "Server=DESKTOP-4LLC9DG;Database=CatFeeding;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return false;
                }
            }

            return true;
        }

        private int GetIdByName(string username)
        {
            using (SqlConnection connection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("CatFeedingDB").Value))
            {
                connection.Open();

                string sqlExpression = $"SELECT Id FROM Users WHERE Login = '{username}';";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
            return 0;
        }

        private IConfigurationRoot GetConnection()
           => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
    }
}
