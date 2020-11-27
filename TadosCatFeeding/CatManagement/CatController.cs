using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly IContext context;

        public CatController(IContext context)
        {
            this.context = context;
        }

        [HttpPost("~/users/{userId}/cats")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(string catName, int userId)
        {
            UserModel user = context.UserRepository.Get(userId);
            if(user == null)
            {
                return NotFound();
            }

            if (user.Login != User.Identity.Name)
            {
                return Forbid("You cannot feed this cat");
            }

            CatModel cat = new CatModel
            {
                OwnerId = userId,
                Name = catName
            };

            int catId = context.CatRepository.Create(cat);
            
            return Created(Url.RouteUrl(catId) + $"/{catId}", catId);
        }
    }    
}
