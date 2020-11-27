using System;

namespace TadosCatFeeding.CatFeedingManagement
{
    public class CatFeedingModel
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public int UserId { get; set; }
        public DateTime FeedingTime { get; set; }
    }
}
