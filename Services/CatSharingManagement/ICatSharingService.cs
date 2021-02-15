using System.Threading.Tasks;

namespace Services.CatSharingManagement
{
    public interface ICatSharingService
    {
        public Task<ServiceResult<CatSharingModel>> ShareAsync(CatSharingCreateModel info, int ownerId);
    }
}
