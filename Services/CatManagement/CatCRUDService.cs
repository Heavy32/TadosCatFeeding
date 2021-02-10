using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Services;

namespace Services.CatManagement
{
    public class CatCRUDService : ICatCRUDService
    {
        private readonly ICatRepository catDatabase;
        private readonly ICatSharingRepository catSharingDatabase;
        private readonly IUserRepository userDatabase;
        private readonly IMapper mapper;

        public CatCRUDService(ICatRepository catDatabase, ICatSharingRepository catSharingDatabase, IUserRepository userDatabase, IMapper mapper)
        {
            this.catDatabase = catDatabase;
            this.catSharingDatabase = catSharingDatabase;
            this.userDatabase = userDatabase;
            this.mapper = mapper;
        }

        public ServiceResult<CatServiceModel> Create(CatCreateServiceModel info)
        {
            UserInDbModel user = userDatabase.Get(info.OwnerId);
            if (user == null)
            {
                return new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            int catId = catDatabase.Create(mapper.Map<CatCreateInDbModel, CatCreateServiceModel>(info));

            CatServiceModel cat = new CatServiceModel(catId, info.Name, info.OwnerId);
            catSharingDatabase.Create(new CatSharingCreateInDbModel(cat.Id, info.OwnerId));

            return new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemCreated, cat);
        }

        public ServiceResult<CatGetServiceModel> Get(int id)
        {
            CatInDbModel cat = catDatabase.Get(id);
            if(cat == null)
            {
                return new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }
            return new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemRecieved, mapper.Map<CatGetServiceModel, CatInDbModel>(cat));
        }
    }
}
