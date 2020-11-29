using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TadosCatFeeding.CatFeedingManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.PetFeedingManagement
{
    [Route("[controller]")]
    [ApiController]
    public class CatFeedingController : ControllerBase
    {
        private readonly IContext context;

        public CatFeedingController(IContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("~/users/{userId}/cats/{catId}/feedings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(int userId, int catId, DateTime feedingTime)
        {
            CatModel cat = context.CatRepository.Get(catId);
            if (cat == null)
            {
                return NotFound("Cat cannot be found");
            }

            UserModel user = context.UserRepository.Get(userId);
            if (user == null)
            {
                return NotFound("User cannot be found");
            }

            if(context.CatSharingRepository.IsPetSharedWithUser(userId, catId))
            {
                return Forbid("You cannot feed this cat");
            }

            CatFeedingModel feeding = new CatFeedingModel
            {
                CatId = catId,
                UserId = userId,
                FeedingTime = feedingTime
            };

            context.CatFeedingRepository.Create(feeding);
            return NoContent();
        }
    }
}
