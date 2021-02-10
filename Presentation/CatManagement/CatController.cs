using Microsoft.AspNetCore.Mvc;
using Services;
using Services.CatManagement;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatCRUDService catService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;

        public CatController(ICatCRUDService catService, IServiceResultStatusToResponseConverter responseConverter)
        {
            this.catService = catService;
            this.responseConverter = responseConverter;
        }

        [HttpPost("~/users/{userId}/cats")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(CatCreateViewModel cat, int userId)
        {
            //how to hide?
            //if (user.Login != User.Identity.Name || !User.IsInRole("Admin"))
            //{
            //    return Forbid("You have no permission to create a cat for another user");
            //}

            return responseConverter.GetResponse(catService.Create(new CatCreateServiceModel(cat.Name, userId)));
        }
    }    
}
