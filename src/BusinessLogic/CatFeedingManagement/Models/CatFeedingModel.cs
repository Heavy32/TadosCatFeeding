using System;

namespace BusinessLogic.CatFeedingManagement
{
    public class CatFeedingModel : IUniqueModel
    {
        public int Id { get; }
        public int CatId { get; }
        public int UserId { get; }
        public DateTime FeedingTime { get; }

        public CatFeedingModel(int id, int catId, int userId, DateTime feedingTime)
        {
            Id = id;
            CatId = catId;
            UserId = userId;
            FeedingTime = feedingTime;
        }
    }
}
