using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.CatFeedingManagement
{
    public class CatFeedingCreateInDbModel
    {
        public int CatId { get; }
        public int UserId { get; }
        public DateTime FeedingTime { get; }

        public CatFeedingCreateInDbModel(int catId, int userId, DateTime feedingTime)
        {
            CatId = catId;
            UserId = userId;
            FeedingTime = feedingTime;
        }
    }
}
