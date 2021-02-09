using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TadosCatFeeding.UserManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TadosCatFeeding.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserEntrance userEntrance;
        private readonly IUserCRUDService userCRUDService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;

        public UserController(IUserEntrance userEntrance, IUserCRUDService userCRUDService, IServiceResultStatusToResponseConverter responseConverter)
        {
            this.userEntrance = userEntrance;
            this.userCRUDService = userCRUDService;
            this.responseConverter = responseConverter;
        }
        
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(UserCreateModel user)
        {
            return responseConverter.GetResponse(userCRUDService.Create(user), Request.Path.Value);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            (string login, string password) = ExtractCredentials(Request);
            return responseConverter.GetResponse(userEntrance.LogIn(login, password));
        }

        [HttpGet("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(int id)
        {         
            return responseConverter.GetResponse(userCRUDService.Get(id));
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id)
        {
            return responseConverter.GetResponse(userCRUDService.Delete(id));
        }

        [HttpPatch("{id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, UserUpdateModel newUserInfo)
        {
            return responseConverter.GetResponse(userCRUDService.Update(id, newUserInfo));
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
