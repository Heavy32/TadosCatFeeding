using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TadosCatFeeding.CRUDoperations;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        [HttpPost("addpet")]
        public IActionResult AddPet(Pet info)
        {
            PetCRUD pet = new PetCRUD();
            (bool success, string report) = pet.Create(info);

            if (success)
            {
                return Ok(report);
            }
            else
            {
                return BadRequest(report);
            }
        }

        [HttpPost]
        [Route("Share")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SharePetWith(UserPet info)
        {
            UsersPetsCRUD userPet = new UsersPetsCRUD();
            (bool success, string report) = userPet.Create(info);

            if (success)
            {
                return Ok(report);
            }
            else
            {
                return BadRequest(report);
            }
        }
    }    
}
