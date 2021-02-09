using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.CatManagement
{
    public class CatCRUDService : ICatCRUDService
    {
        private readonly CatRepository catDatabase;
        private readonly CatSharingRepository catSharingDatabase;
        private readonly UserRepository userDatabase;

        public CatCRUDService(CatRepository catDatabase, CatSharingRepository catSharingDatabase, UserRepository userDatabase)
        {
            this.userDatabase = userDatabase;
            this.catDatabase = catDatabase;
            this.catSharingDatabase = catSharingDatabase;
        }

        public ServiceResult<CatModel> Create(CatCreateModel info)
        {
            UserInDB user = userDatabase.Get(info.OwnerId);
            if (user == null)
            {
                return new ServiceResult<CatModel>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            int catId = catDatabase.Create(info);

            CatModel cat = new CatModel(catId, info.Name, info.OwnerId);
            catSharingDatabase.Create(new CatUserLink(cat.Id, info.OwnerId));

            return new ServiceResult<CatModel>(ServiceResultStatus.ItemCreated, cat);
        }

        public ServiceResult<CatModel> Get(int id)
        {
            CatModel cat = catDatabase.Get(id);
            if(cat == null)
            {
                return new ServiceResult<CatModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }
            return new ServiceResult<CatModel>(ServiceResultStatus.ItemRecieved, cat);
        }
    }
}
