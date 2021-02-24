namespace DataBaseRepositories.CatSharingRepository
{
    public class CatSharingCreateInDbModel
    {
        public int CatId { get; }
        public int UserId { get; }

        public CatSharingCreateInDbModel(int catId, int userId)
        {
            CatId = catId;
            UserId = userId;
        }
    }
}
