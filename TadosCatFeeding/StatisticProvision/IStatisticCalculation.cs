using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.StatisticProvision
{
    public interface IStatisticCalculation
    {
        public StatisticResult Execute(string sqlExpression);
    }
}
