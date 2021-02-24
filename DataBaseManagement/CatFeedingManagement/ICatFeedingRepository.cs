using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBaseManagement.CatFeedingManagement
{
    public interface ICatFeedingRepository
    {
        public Task<int> CreateAsync(CatFeedingCreateInDbModel info);
        public Task<List<DateTime>> GetFeedingForPeriodAsync(int userId, int catId, DateTime start, DateTime finish);
    }
}
