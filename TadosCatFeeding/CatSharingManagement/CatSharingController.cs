using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.CatSharingManagement;

namespace Presentation.PetSharingManagement
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
        public IActionResult Share(int userId, int catId, int UserToShare)
        {
            return responseConverter.GetResponse(catSharingService.Share(new CatSharingCreateModel(catId, UserToShare), userId));
        }
    }
}
