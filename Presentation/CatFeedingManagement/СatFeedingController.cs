using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.CatFeedingManagement;

namespace Presentation
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Feed(int userId, int catId, DateTime feedingTime)
        {
            return responseConverter.GetResponse(catFeedingService.Feed(new CatFeedingCreateModel(catId, userId, feedingTime)));
        }

        [HttpGet("~/users/{userId}/cats/{catId}/feedings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
        {
            return responseConverter.GetResponse(catFeedingService.GetFeedingForPeriod(userId, catId, start, finish));
        }
    }
}
