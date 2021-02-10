using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;

namespace Services.CatSharingManagement
{
    public class CatSharingService : ICatSharingService
    {
        private readonly ICatSharingRepository catSharingDatabase;
        private readonly ICatRepository catDatabase;
        private readonly IUserRepository userDatabase;
        private readonly IMapper mapper;

        public CatSharingService(ICatSharingRepository catSharingDatabase, ICatRepository catDatabase, IUserRepository userDatabase, IMapper mapper)
        {
            this.catSharingDatabase = catSharingDatabase;
            this.catDatabase = catDatabase;
            this.userDatabase = userDatabase;
            this.mapper = mapper;
        }

        public ServiceResult<CatSharingModel> Share(CatSharingCreateModel info, int ownerId)
        {
            UserInDbModel userInDb = userDatabase.Get(info.UserId);
            if (userInDb == null)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "UserToShare cannot be found");
            }

            CatInDbModel cat = catDatabase.Get(info.CatId);
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
                catSharingDatabase.Create(mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(info));
            }
            return new ServiceResult<CatSharingModel>(ServiceResultStatus.PetIsShared);
        }

        public bool IsPetSharedWithUser(int userId, int petId)
        {
            return catSharingDatabase.IsPetSharedWithUser(userId, petId);
        }
    }
}
