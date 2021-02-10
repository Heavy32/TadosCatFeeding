namespace Services.CatSharingManagement
{
    public class CatSharingModel : IUniqueModel
    {
        public CatSharingModel(int id, int catId, int userId)
        {
            Id = id;
            CatId = catId;
            UserId = userId;
        }

        public int Id { get; }
        public int CatId { get; }
        public int UserId { get; }
    }
}
