using System;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.CatFeedingManagement;

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
            return responseConverter.GetResponse(catFeedingService.Feed(new CatFeedingCreateModel(catId, userId, feedingTime)));
        }

        [HttpGet("~/users/{userId}/cats/{catId}/feedings")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
        {
            return responseConverter.GetResponse(catFeedingService.GetFeedingForPeriod(userId, catId, start, finish));
        }
    }
}
