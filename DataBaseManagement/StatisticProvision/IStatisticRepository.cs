using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBaseManagement.StatisticProvision
{
    public interface IStatisticRepository
    {
        public Task<int> CreateAsync(StatisticInDbModel info);
        public Task<StatisticInDbModel> GetAsync(int id);
        public Task<List<StatisticInDbModel>> GetAllAsync();
    }
}
