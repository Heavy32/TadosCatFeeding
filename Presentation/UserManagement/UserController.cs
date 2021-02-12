﻿using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UserManagement;
using Services;
using Presentation.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Presentation.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserEntrance userEntrance;
        private readonly IUserCRUDService userCRUDService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;
        private readonly IMapper mapper;

        public UserController(IUserEntrance userEntrance, IUserCRUDService userCRUDService, IServiceResultStatusToResponseConverter responseConverter, IMapper mapper)
        {
            this.userEntrance = userEntrance;
            this.userCRUDService = userCRUDService;
            this.responseConverter = responseConverter;
            this.mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserServiceModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult Create(UserCreateViewModel user)
        {
            return responseConverter.
                GetResponse(
                userCRUDService.Create(
                    mapper.Map<UserCreateModel, UserCreateViewModel>(user)), Request.Path.Value);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TokenJwt), 200)]
        [ProducesResponseType(401)]
        public IActionResult LogIn()
        {
            (string login, string password) = ExtractCredentials(Request);
            return responseConverter.GetResponse(userEntrance.LogIn(login, password));
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {         
            return responseConverter.GetResponse(userCRUDService.Get(id));
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            return responseConverter.GetResponse(userCRUDService.Delete(id, User.Claims));
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult Update(int id, UserUpdateViewModel newUserInfo)
        {
            return responseConverter.
                GetResponse(
                userCRUDService.Update(
                    id, mapper.Map<UserUpdateModel, UserUpdateViewModel>(newUserInfo),
                    User.Claims));
        }

        private (string user, string password) ExtractCredentials(HttpRequest request)
        {
            string authHeader = request.Headers["Authorization"];

            string encodedUsernamePassword = authHeader["Basic ".Length..].Trim();

            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var t = usernamePassword.Split(':');

            return (t[0], t[1]);
        }
    }
}
