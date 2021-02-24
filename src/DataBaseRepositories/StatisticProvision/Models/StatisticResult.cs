using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseRepositories.StatisticProvision
{
    public class StatisticResult
    {
        public List<Dictionary<string, object>> Results { get; }

        public StatisticResult(List<Dictionary<string, object>> results)
        {
            Results = results;
        }
    }
}
