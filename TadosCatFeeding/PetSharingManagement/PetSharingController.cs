using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TadosCatFeeding.PetSharingManagement
{
    [Route("[controller]")]
    [ApiController]
    public class PetSharingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("get test");
        }

        [HttpPost]
       public IActionResult Post()
        {
            return Ok("post test");
        }

        [HttpPut]
        public IActionResult Put()
        {
            return Ok("Put test");
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok("Delete test");
        }
    }
}
