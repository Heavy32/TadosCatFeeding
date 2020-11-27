using TadosCatFeeding.CatFeedingManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.StatisticProvision;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding
{
    public class Context : IContext
    {
        public IUserRepository UserRepository { get; set; }
        public ICatRepository CatRepository { get; set; }
        public ICatFeedingRepository CatFeedingRepository { get; set; }
        public ICatSharingRepository CatSharingRepository { get; set; }
        public IStatisticRepository StatisticRepository { get; set; }

        public Context(string connectionString)
        {
            UserRepository = new UserRepository(connectionString);
            CatRepository = new CatRepository(connectionString);
            CatFeedingRepository = new CatFeedingRepository(connectionString);
            CatSharingRepository = new CatSharingRepository(connectionString);
            StatisticRepository = new StatisticRepository(connectionString);
        }
    }
}
