using DataBaseRepositories.CatRepository;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.UserManagement;
using BusinessLogic.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLogic.CatManagement
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

        public async Task<ServiceResult<CatServiceModel>> CreateAsync(CatCreateServiceModel info, IEnumerable<Claim> userClaims)
        {
            int userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            Roles userRole = (Roles)Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value);

            if (userId != info.OwnerId)
            {
                if (userRole != Roles.Admin)
                {
                    return new ServiceResult<CatServiceModel>(ServiceResultStatus.ActionNotAllowed, "You cannot create cat for this user");
                }
            }

            UserInDbModel user = await userDatabase.GetAsync(info.OwnerId);
            if (user == null)
            {
                return new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            int catId = await catDatabase.CreateAsync(mapper.Map<CatCreateInDbModel, CatCreateServiceModel>(info));

            CatServiceModel cat = new CatServiceModel(catId, info.Name, info.OwnerId);
            await catSharingDatabase.CreateAsync(new CatSharingCreateInDbModel(cat.Id, info.OwnerId));

            return new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemCreated, cat);
        }

        public async Task<ServiceResult<CatGetServiceModel>> GetAsync(int id)
        {
            CatInDbModel cat = await catDatabase.GetAsync(id);
            if(cat == null)
            {
                return new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }
            return new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemRecieved, mapper.Map<CatGetServiceModel, CatInDbModel>(cat));
        }
    }
}
