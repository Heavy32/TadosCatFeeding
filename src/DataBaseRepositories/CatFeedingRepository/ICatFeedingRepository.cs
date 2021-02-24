using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBaseRepositories.CatFeedingRepository
{
    public interface ICatFeedingRepository
    {
        public Task<int> CreateAsync(CatFeedingCreateInDbModel info);
        public Task<List<CatFeedingInDbModel>> GetFeedingsForPeriodAsync(int userId, int catId, DateTime start, DateTime finish);
    }
}
