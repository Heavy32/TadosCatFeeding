using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TadosCatFeeding.UserEntranceManagement
{
    [Route("[controller]")]
    [ApiController]
    public class UserEntranceController : ControllerBase
    {
        [HttpGet("logIn")]
        public IActionResult Get()
        {
            return Ok("get test");
        }

        [HttpPost("signUp")]
        public IActionResult Post()
        {
            return Ok("post test");
        }
    }
}
