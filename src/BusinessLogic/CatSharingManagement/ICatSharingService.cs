using System.Threading.Tasks;

namespace BusinessLogic.CatSharingManagement
{
    public interface ICatSharingService
    {
        public Task<ServiceResult<CatSharingModel>> ShareAsync(CatSharingCreateModel info, int ownerId);
    }
}
