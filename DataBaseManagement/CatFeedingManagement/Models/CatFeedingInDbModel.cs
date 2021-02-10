using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.CatFeedingManagement
{
    public class CatFeedingInDbModel
    {
        public int Id { get; }
        public int CatId { get; }
        public int UserId { get; }
        public DateTime FeedingTime { get; }

        public CatFeedingInDbModel(int id, int catId, int userId, DateTime feedingTime)
        {
            Id = id;
            CatId = catId;
            UserId = userId;
            FeedingTime = feedingTime;
        }
    }
}
