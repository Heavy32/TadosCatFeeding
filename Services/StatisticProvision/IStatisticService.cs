using DataBaseManagement.StatisticProvision;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.StatisticProvision
{
    public interface IStatisticService
    {
        public Task<ServiceResult<StatisticResult>> GetStatisticResultAsync(int id);
        public Task<ServiceResult<List<StatisticModel>>> GetAllAsync();
    }
}
