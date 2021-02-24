using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.CatSharingManagement;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatSharingController : ControllerBase
    {
        private readonly ICatSharingService catSharingService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;

        public CatSharingController(ICatSharingService catSharingService, IServiceResultStatusToResponseConverter responseConverter)
        {
            this.catSharingService = catSharingService;
            this.responseConverter = responseConverter;
        }

        [HttpPut("~/users/{userId}/cats/{catId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ShareAsync(int userId, int catId, int UserToShare)
        {
            return responseConverter.GetResponse(await catSharingService.ShareAsync(new CatSharingCreateModel(catId, UserToShare), userId));
        }
    }
}
