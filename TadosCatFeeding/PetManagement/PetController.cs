using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TadosCatFeeding.CRUDoperations;
using TadosCatFeeding.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TadosCatFeeding.Controllers
{
    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    [Route("[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create(string message)
        {
            List<TestClass> a = new List<TestClass> { new TestClass { Id = 1, Name = "qwe" } };
            return Ok(a);
        }

        [HttpGet]
        public IActionResult Get(int a)
        {
            return Ok(a);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update(int id, TestClass testClass)
        {
            return Ok($"{testClass.Id} {testClass.Name} Updated");
        }
    }    
}
