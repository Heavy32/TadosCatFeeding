using System.Threading.Tasks;

namespace DataBaseRepositories.CatSharingRepository
{
    public interface ICatSharingRepository
    {
        public Task<int> CreateAsync(CatSharingCreateInDbModel info);
        public Task<bool> IsPetSharedWithUserAsync(int userId, int petId);
    }
}
