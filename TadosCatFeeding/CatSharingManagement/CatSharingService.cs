using TadosCatFeeding.CatManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.CatSharingManagement
{
    public class CatSharingService : ICatSharingService
    {
        private readonly CatSharingRepository catSharingDatabase;
        private readonly CatRepository catDatabase;
        private readonly UserRepository userDatabase;

        public CatSharingService(CatSharingRepository catSharingDatabase, CatRepository catDatabase, UserRepository userDatabase)
        {
            this.catSharingDatabase = catSharingDatabase;
            this.catDatabase = catDatabase;
            this.userDatabase = userDatabase;
        }

        public ServiceResult<CatSharingModel> Share(CatUserLink info, int ownerId)
        {
            UserInDB userInDb = userDatabase.Get(info.UserId);
            if (userInDb == null)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "UserToShare cannot be found");
            }

            CatModel cat = catDatabase.Get(info.CatId);
            if(cat == null)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }
            if(cat.OwnerId != ownerId)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.CantShareWithUser, "This user cannot share the pet");
            }

            if(!IsPetSharedWithUser(info.UserId, info.CatId))
            {
                catSharingDatabase.Create(info);
            }
            return new ServiceResult<CatSharingModel>(ServiceResultStatus.PetIsShared);
        }

        public bool IsPetSharedWithUser(int userId, int petId)
        {
            return catSharingDatabase.IsPetSharedWithUser(userId, petId);
        }
    }
}
