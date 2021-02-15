using System.Threading.Tasks;

namespace DataBaseManagement.CatManagement
{
    public interface ICatRepository
    {
        public Task<int> CreateAsync(CatCreateInDbModel info);
        public Task<CatInDbModel> GetAsync(int id);
    }
}
