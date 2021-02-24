using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.CatFeedingManagement;

namespace WebApi.Controllers
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
        public async Task<IActionResult> FeedAsync(int userId, int catId, DateTime feedingTime)
        {
            return responseConverter.GetResponse(await catFeedingService.FeedAsync(new CatFeedingCreateModel(catId, userId, feedingTime)));
        }

        [HttpGet("~/users/{userId}/cats/{catId}/feedings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetFeedingForPeriodAsync(int userId, int catId, DateTime start, DateTime finish)
        {
            return responseConverter.GetResponse(await catFeedingService.GetFeedingForPeriodAsync(userId, catId, start, finish));
        }
    }
}
