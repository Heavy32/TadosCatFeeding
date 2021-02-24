using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.CatFeedingManagement
{
    public interface ICatFeedingService
    {
        public Task<ServiceResult<CatFeedingModel>> FeedAsync(CatFeedingCreateModel info);
        public Task<ServiceResult<List<DateTime>>> GetFeedingForPeriodAsync(int userId, int catId, DateTime start, DateTime finish);
    }
}
