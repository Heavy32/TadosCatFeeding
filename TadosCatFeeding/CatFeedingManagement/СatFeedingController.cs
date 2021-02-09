using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TadosCatFeeding.CatFeedingManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.PetFeedingManagement
{
    [Route("[controller]")]
    [ApiController]
    public class CatFeedingController : ControllerBase
    {
        private readonly ICatFeedingService catFeedingService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;

        public CatFeedingController(ICatFeedingService catFeedingService, IServiceResultStatusToResponseConverter responseConverter)
        {
            this.catFeedingService = catFeedingService;
            this.responseConverter = responseConverter;
        }

        [HttpPost("~/users/{userId}/cats/{catId}/feedings")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Feed(int userId, int catId, DateTime feedingTime)
        {
            return responseConverter.GetResponse(catFeedingService.Feed(new CatFeedingCreateModel(catId, userId, DateTime.Now)));
        }
    }
}
