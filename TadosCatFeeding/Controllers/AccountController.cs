using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TadosCatFeeding.CRUDoperations;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("LogIn")]
        [AllowAnonymous]
        public async Task LogIn(Account userInfo)
        {
            ClaimsIdentity identity = GetIdentity(userInfo.Login, userInfo.Password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            await GetToken(identity);
        }

        [HttpPost("SignUp")]
        [AllowAnonymous]
        public IActionResult SignUp(Account userInfo)
        {
            AccountCRUD account = new AccountCRUD();
            (bool success, string report) = account.Create(userInfo);

            if (success)
            {
                return Ok(report);
            }
            else
            {
                return BadRequest(report);
            }
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            string role;

            using (SqlConnection connection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("CatFeedingDB").Value))
            {
                connection.Open();

                string sqlExpression = $"SELECT Password, Role FROM Users WHERE Login = '{username}';";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string passwordFromDB = reader.GetString(0);
                    role = reader.GetString(1);

                    if (passwordFromDB != password)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
            };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private async Task GetToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }
    }
}
