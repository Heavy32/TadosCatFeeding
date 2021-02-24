using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CatFeedingManagement
{
    public class CatFeedingCreateModel
    {
        public int CatId { get; }
        public int UserId { get; }
        public DateTime FeedingTime { get; }

        public CatFeedingCreateModel(int catId, int userId, DateTime feedingTime)
        {
            CatId = catId;
            UserId = userId;
            FeedingTime = feedingTime;
        }
    }
}
