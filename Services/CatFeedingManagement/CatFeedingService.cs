using DataBaseManagement.CatFeedingManagement;
using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Services;
using System;
using System.Collections.Generic;

namespace Services.CatFeedingManagement
{
    public class CatFeedingService : ICatFeedingService
    {
        private readonly ICatFeedingRepository catFeedingdatabase;
        private readonly ICatRepository catDatabase;
        private readonly IUserRepository userDatabase;
        private readonly ICatSharingRepository catSharingDatabase;
        private readonly IMapper mapper;

        public CatFeedingService(ICatFeedingRepository catFeedingdatabase, ICatRepository catDatabase, IUserRepository userDatabase, ICatSharingRepository catSharingDatabase, IMapper mapper)
        {
            this.catFeedingdatabase = catFeedingdatabase;
            this.catDatabase = catDatabase;
            this.userDatabase = userDatabase;
            this.catSharingDatabase = catSharingDatabase;
            this.mapper = mapper;
        }

        public ServiceResult<CatFeedingModel> Feed(CatFeedingCreateModel info)
        {
            CatInDbModel cat = catDatabase.Get(info.CatId);
            if (cat == null)
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }

            UserInDbModel user = userDatabase.Get(info.UserId);
            if (user == null)
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            if (!catSharingDatabase.IsPetSharedWithUser(info.UserId, info.CatId))
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ActionNotAllowed, "You cannot feed this cat");
            }

            catFeedingdatabase.Create(mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(info));

            return new ServiceResult<CatFeedingModel>(ServiceResultStatus.NoContent);
        }

        public ServiceResult<List<DateTime>> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
        {
            UserInDbModel user = userDatabase.Get(userId);
            if(user == null)
            {
                return new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            CatInDbModel cat = catDatabase.Get(catId);
            if(cat == null)
            {
                return new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }

            return new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemRecieved, catFeedingdatabase.GetFeedingForPeriod(userId, catId, start, finish));
        }
    }
}
