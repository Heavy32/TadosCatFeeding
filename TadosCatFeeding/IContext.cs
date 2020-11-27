using TadosCatFeeding.CatFeedingManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.StatisticProvision;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding
{
    public interface IContext
    {
        IUserRepository UserRepository { get; set; }
        ICatRepository CatRepository { get; set; }
        ICatFeedingRepository CatFeedingRepository { get; set; }
        ICatSharingRepository CatSharingRepository { get; set; }
        IStatisticRepository StatisticRepository { get; set; }
    }
}
