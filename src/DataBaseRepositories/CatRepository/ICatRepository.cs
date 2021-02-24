using System.Threading.Tasks;

namespace DataBaseRepositories.CatRepository
{
    public interface ICatRepository
    {
        public Task<int> CreateAsync(CatCreateInDbModel info);
        public Task<CatInDbModel> GetAsync(int id);
    }
}
