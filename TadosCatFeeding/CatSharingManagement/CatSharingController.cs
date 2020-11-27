using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        [HttpPut("users/{userId}/cats/{catId}")]
        public IActionResult Share(int userId, int catId)
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
                return NoContent();
            }

            CatSharingModel link = new CatSharingModel
            {
                UserId = userId,
                CatId = catId,
            };

            context.CatSharingRepository.Create(link);
            return NoContent();
        }
    }
}
