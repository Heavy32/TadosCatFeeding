using DataBaseManagement.StatisticProvision;
using Services;
using System.Collections.Generic;
using System.Linq;

namespace Services.StatisticProvision
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository statisticRepository;
        private readonly IStatisticCalculation calculation;
        private readonly IMapper mapper;

        public StatisticService(IStatisticRepository statisticRepository, IStatisticCalculation calculation, IMapper mapper)
        {
            this.statisticRepository = statisticRepository;
            this.calculation = calculation;
            this.mapper = mapper;
        }

        public StatisticResult Execute(string sqlExpression)
        {
            return calculation.Execute(sqlExpression);
        }

        public ServiceResult<StatisticResult> GetStatisticResult(int id)
        {
            StatisticInDbModel statistic = statisticRepository.Get(id);

            if (statistic == null)
            {
                return new ServiceResult<StatisticResult>(ServiceResultStatus.ItemNotFound);
            }

            return new ServiceResult<StatisticResult>(ServiceResultStatus.ItemRecieved, Execute(statistic.SqlExpression));
        }

        public ServiceResult<List<StatisticModel>> GetAll()
        {
            List<StatisticInDbModel> statistics = statisticRepository.GetAll();

            if(statistics.Count == 0)
            {
                return new ServiceResult<List<StatisticModel>>(ServiceResultStatus.NoContent);
            }

            return new ServiceResult<List<StatisticModel>>(ServiceResultStatus.ItemRecieved, statistics.Select(model => mapper.Map<StatisticModel, StatisticInDbModel>(model)).ToList());
        }
    }
}
