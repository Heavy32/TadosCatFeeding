using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.PetSharingManagement
{
    [Route("[controller]")]
    [ApiController]
    public class CatSharingController : ControllerBase
    {
        private readonly IContext context;

        public CatSharingController(IContext context)
        {
            this.context = context;
        }

        [HttpPut("~/users/{userId}/cats/{catId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Share(int userId, int catId, int ShareWithUserId)
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

            if (!context.CatSharingRepository.IsPetSharedWithUser(ShareWithUserId, catId))
            {
                CatSharingModel link = new CatSharingModel
                {
                    UserId = ShareWithUserId,
                    CatId = catId,
                };

                context.CatSharingRepository.Create(link);
            }

            return NoContent();
        }
    }
}
