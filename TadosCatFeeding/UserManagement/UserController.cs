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

namespace TadosCatFeeding.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IContext context;

        public UserController(IContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(UserModel user)
        {
            int id =context.UserRepository.Create(user);

            var body = new
            {
                token = GetToken(GetIdentity(user)),
                id
            };

            return Created(Url.RouteUrl(id) + $"/{id}", body);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            (string login, string password) = ExtractCredentials(Request);

            UserModel user = context.UserRepository.GetUserByLogindAndPassword(login, password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok($"Token : {GetToken(GetIdentity(user))},\n UserId : {user.Id}");
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(int id)
        {
            UserModel user = context.UserRepository.Get(id);

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
            UserModel user = context.UserRepository.Get(userId);
            if (user == null)
            {
                return NotFound();
            }

            context.UserRepository.Delete(userId);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, UserModel changedInfo)
        {
            UserModel user = context.UserRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            UserModel newUser = new UserModel
            {
                Id = user.Id,
                Login = user.Login == changedInfo.Login ? user.Login : changedInfo.Login,
                Password = user.Password == changedInfo.Password ? user.Password : changedInfo.Password,
                Nickname = user.Nickname == changedInfo.Nickname ? user.Nickname : changedInfo.Nickname,
                Role = user.Role == changedInfo.Role ? user.Role : changedInfo.Role,
            };

            context.UserRepository.Update(id, newUser);

            return NoContent();
        }

        private (string user, string password) ExtractCredentials(HttpRequest request)
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var t = usernamePassword.Split(':');

            return (t[0], t[1]);
        }

        private ClaimsIdentity GetIdentity(UserModel user)
        {    
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private string GetToken(ClaimsIdentity identity)
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

            return encodedJwt;
        }
    }
}
