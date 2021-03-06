﻿using DataBaseRepositories.CatFeedingRepository;
using DataBaseRepositories.CatRepository;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.CatFeedingManagement
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

        public async Task<ServiceResult<CatFeedingModel>> FeedAsync(CatFeedingCreateModel info)
        {
            CatInDbModel cat = await catDatabase.GetAsync(info.CatId);
            if (cat == null)
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }

            UserInDbModel user = await userDatabase.GetAsync(info.UserId);
            if (user == null)
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            if (!await catSharingDatabase.IsCatSharedWithUserAsync(info.UserId, info.CatId))
            {
                return new ServiceResult<CatFeedingModel>(ServiceResultStatus.ActionNotAllowed, "You cannot feed this cat");
            }

            await catFeedingdatabase.CreateAsync(mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(info));

            return new ServiceResult<CatFeedingModel>(ServiceResultStatus.NoContent);
        }

        public async Task<ServiceResult<List<DateTime>>> GetFeedingForPeriodAsync(int userId, int catId, DateTime start, DateTime finish)
        {
            UserInDbModel user = await userDatabase.GetAsync(userId);
            if(user == null)
            {
                return new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "User is not found");
            }

            CatInDbModel cat = await catDatabase.GetAsync(catId);
            if(cat == null)
            {
                return new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "Cat is not found");
            }

            List<DateTime> info = new List<DateTime>();
            List<CatFeedingInDbModel> feedings = await catFeedingdatabase.GetFeedingsForPeriodAsync(userId, catId, start, finish);
            for (int i = 0; i < feedings.Count; i++)
            {
                info.Add(feedings[i].FeedingTime);
            }
            return new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemRecieved, info);
        }
    }
}
