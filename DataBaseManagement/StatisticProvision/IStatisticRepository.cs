using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.StatisticProvision
{
    public interface IStatisticRepository
    {
        public int Create(StatisticModel info);
        public StatisticModel Get(int id);
        public List<StatisticModel> GetAll();
    }
}
