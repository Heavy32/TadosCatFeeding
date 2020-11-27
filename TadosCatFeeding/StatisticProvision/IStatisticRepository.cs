using System;
using System.Collections.Generic;
using TadosCatFeeding.Abstractions;

namespace TadosCatFeeding.StatisticProvision
{
    public interface IStatisticRepository : IRepository<StatisticModel>
    {
        public List<DateTime> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish);
    }
}
