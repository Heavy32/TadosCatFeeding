using System.Threading.Tasks;

namespace DataBaseManagement.CatSharingManagement
{
    public interface ICatSharingRepository
    {
        public Task<int> CreateAsync(CatSharingCreateInDbModel info);
        public Task<bool> IsPetSharedWithUserAsync(int userId, int petId);
    }
}
