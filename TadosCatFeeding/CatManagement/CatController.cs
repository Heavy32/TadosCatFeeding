using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.Controllers
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
        public IActionResult Create(CatInputFromResponseModel catName, int userId)
        {
            //how to hide?
            //if (user.Login != User.Identity.Name || !User.IsInRole("Admin"))
            //{
            //    return Forbid("You have no permission to create a cat for another user");
            //}

            return responseConverter.GetResponse(catService.Create(new CatCreateModel(catName.Name, userId)));
        }
    }    
}
