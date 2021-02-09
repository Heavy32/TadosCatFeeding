using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.StatisticProvision
{
    public interface IStatisticCRUDService
    {
        public ServiceResult<StatisticResult> GetStatisticResult(int id);
        public ServiceResult<List<StatisticModel>> GetAll();
    }
}
