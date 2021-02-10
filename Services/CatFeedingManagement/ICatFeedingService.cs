using System;
using System.Collections.Generic;

namespace Services.CatFeedingManagement
{
    public interface ICatFeedingService
    {
        public ServiceResult<CatFeedingModel> Feed(CatFeedingCreateModel info);
        public ServiceResult<List<DateTime>> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish);
    }
}
