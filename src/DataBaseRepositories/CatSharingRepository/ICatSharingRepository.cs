using System.Threading.Tasks;

namespace DataBaseRepositories.CatSharingRepository
{
    public interface ICatSharingRepository
    {
        public Task<int> CreateAsync(CatSharingCreateInDbModel info);
        public Task<bool> IsCatSharedWithUserAsync(int userId, int catId);
    }
}
