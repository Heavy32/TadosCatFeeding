using System;
using System.Collections.Generic;

namespace DataBaseManagement.CatFeedingManagement
{
    public interface ICatFeedingRepository
    {
        public int Create(CatFeedingCreateInDbModel info);
        public List<DateTime> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish);
    }
}
