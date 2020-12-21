using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TadosCatFeeding.Models;
using TadosCatFeeding.UserManagement;
using System.Text.Json;

namespace TadosCatFeeding.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserEntrance userEntrance;
        private readonly UserCRUDService userCRUDService;

        public UserController(UserEntrance userEntrance, UserCRUDService userCRUDService)
        {
            this.userEntrance = userEntrance;
            this.userCRUDService = userCRUDService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(UserModel user)
        {
            int id = userCRUDService.Create(user);

            var responseBody = new
            {
                userEntrance.LogIn(user.Login, user.Password).token,
                id
            };

            return Created(Url.RouteUrl(id) + $"/{id}", JsonSerializer.Serialize(responseBody));
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            (string login, string password) = ExtractCredentials(Request);

            (string token, int userId) responseBody = userEntrance.LogIn(login, password);

            if (responseBody.token == null)
            {
                return Unauthorized();
            }

            return Ok(JsonSerializer.Serialize(responseBody));
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(int id)
        {
            UserModel user = userCRUDService.Get(id);

            //should be a validation
            if (user == null)
            {
                return NotFound();
            }

            if (user.Login != User.Identity.Name)
            {
                return Forbid("You cannot get information about this user");
            }

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int userId)
        {
            //should be a validation
            UserModel user = userCRUDService.Get(userId);
            if (user == null)
            {
                return NotFound();
            }

            userCRUDService.Delete(userId);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, NewUserInfo newUserInfo)
        {
            //should be a validation
            UserModel user = userCRUDService.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            //should I map newUserInfo to UserModel?
            userCRUDService.Update(id,
                new UserModel
                {
                    Id = newUserInfo.Id,
                    Login = newUserInfo.Login,
                    Password = newUserInfo.Password,
                    Nickname = newUserInfo.Nickname
                });

            return NoContent();
        }

        private (string user, string password) ExtractCredentials(HttpRequest request)
        {
            string authHeader = request.Headers["Authorization"];

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var t = usernamePassword.Split(':');

            return (t[0], t[1]);
        }
    }
}
