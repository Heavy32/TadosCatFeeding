using TadosCatFeeding.Abstractions;

namespace TadosCatFeeding.CatSharingManagement
{
    public interface ICatSharingRepository : IRepository<CatSharingModel>
    {
        bool IsPetSharedWithUser(int userId, int petId);
    }
}
