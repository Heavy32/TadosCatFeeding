using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.CatFeedingManagement
{
    public class CatFeedingService : ICatFeedingService
    {
        private readonly ICatFeedingRepository database;
        private readonly ICatRepository catDatabase;
        private readonly IUserRepository userDatabase;
        private readonly ICatSharingRepository catSharingDatabase;

        public CatFeedingService(ICatFeedingRepository database, ICatRepository catDatabase, IUserRepository userDatabase, ICatSharingRepository catSharingDatabase)
        {
            this.database = database;
            this.catDatabase = catDatabase;
            this.userDatabase = userDatabase;
            this.catSharingDatabase = catSharingDatabase;
        }

        public ServiceResult<CatFeedingModel> Feed(CatFeedingCreateModel info)
        {
            CatModel cat = catDatabase.Get(info.CatId);
            if (cat == null)
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "Cat cannot be found");
            }

            UserInDB user = userDatabase.Get(info.UserId);
            if (user == null)
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            if (!catSharingDatabase.IsPetSharedWithUser(info.UserId, info.CatId))
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.PetIsNotShared, "You cannot feed this cat");
            }

            database.Create(info);

            return new ServiceResult<CatFeedingModel>(ServiceResultStatus.NoContent);
        }
    }
}
