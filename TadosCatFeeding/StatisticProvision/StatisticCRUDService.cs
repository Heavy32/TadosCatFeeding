using System.Collections.Generic;

namespace TadosCatFeeding.StatisticProvision
{
    public class StatisticCRUDService : IStatisticCRUDService
    {
        private readonly IStatisticRepository statisticRepository;
        private readonly IStatisticCalculation calculation;

        public StatisticCRUDService(IStatisticRepository statisticRepository, IStatisticCalculation calculation)
        {
            this.statisticRepository = statisticRepository;
            this.calculation = calculation;
        }

        public ServiceResult<StatisticResult> GetStatisticResult(int id)
        {
            StatisticModel statistic = statisticRepository.Get(id);

            if (statistic == null)
            {
                return new ServiceResult<StatisticResult>(ServiceResultStatus.ItemNotFound);
            }

            return new ServiceResult<StatisticResult>(ServiceResultStatus.ItemRecieved, calculation.Execute(statistic.SqlExpression));
        }

        public ServiceResult<List<StatisticModel>> GetAll()
        {
            List<StatisticModel> statistics = statisticRepository.GetAll();

            if(statistics.Count == 0)
            {
                return new ServiceResult<List<StatisticModel>>(ServiceResultStatus.NoContent);
            }

            return new ServiceResult<List<StatisticModel>>(ServiceResultStatus.ItemRecieved, statistics);
        }
    }
}
