using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.StatisticProvision
{
    public interface IStatisticRepository
    {
        public int Create(StatisticInDbModel info);
        public StatisticInDbModel Get(int id);
        public List<StatisticInDbModel> GetAll();
    }
}
