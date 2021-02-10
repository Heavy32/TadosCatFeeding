using DataBaseManagement.StatisticProvision;
using System.Collections.Generic;

namespace Services.StatisticProvision
{
    public interface IStatisticService
    {
        public ServiceResult<StatisticResult> GetStatisticResult(int id);
        public ServiceResult<List<StatisticModel>> GetAll();
    }
}
