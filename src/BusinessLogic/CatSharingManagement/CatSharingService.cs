using DataBaseRepositories.CatRepository;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.UserManagement;
using System.Threading.Tasks;

namespace BusinessLogic.CatSharingManagement
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

        public async Task<ServiceResult<CatSharingModel>> ShareAsync(CatSharingCreateModel info, int ownerId)
        {
            UserInDbModel userInDb = await userDatabase.GetAsync(info.UserId);
            if (userInDb == null)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "User to share cannot be found");
            }

            CatInDbModel cat = await catDatabase.GetAsync(info.CatId);
            if(cat == null)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }

            if(cat.OwnerId != ownerId)
            {
                return new ServiceResult<CatSharingModel>(ServiceResultStatus.CantShareWithUser, "This user cannot share the pet");
            }

            if(!await IsPetSharedWithUser(info.UserId, info.CatId))
            {
                await catSharingDatabase.CreateAsync(mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(info));
            }
            return new ServiceResult<CatSharingModel>(ServiceResultStatus.PetIsShared);
        }

        public async Task<bool> IsPetSharedWithUser(int userId, int petId)
        {
            return await catSharingDatabase.IsPetSharedWithUserAsync(userId, petId);
        }
    }
}
