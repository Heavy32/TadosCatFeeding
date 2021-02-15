using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CatManagement;
using System.Threading.Tasks;

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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateAsync(CatCreateViewModel cat, int userId)
        {
            return responseConverter.GetResponse(await catService.CreateAsync(new CatCreateServiceModel(cat.Name, userId), User.Claims));
        }
    }    
}
