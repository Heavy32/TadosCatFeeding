using DataBaseRepositories.StatisticProvision;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.StatisticProvision
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

        public async Task<StatisticResult> ExecuteAsync(string sqlExpression)
        {
            return await calculation.ExecuteAsync(sqlExpression);
        }

        public async Task<ServiceResult<StatisticResult>> GetStatisticResultAsync(int id)
        {
            StatisticInDbModel statistic = await statisticRepository.GetAsync(id);

            if (statistic == null)
            {
                return new ServiceResult<StatisticResult>(ServiceResultStatus.ItemNotFound);
            }

            return new ServiceResult<StatisticResult>(ServiceResultStatus.ItemRecieved, await ExecuteAsync(statistic.SqlExpression));
        }

        public async Task<ServiceResult<List<StatisticModel>>> GetAllAsync()
        {
            List<StatisticInDbModel> statistics = await statisticRepository.GetAllAsync();

            if(statistics.Count == 0)
            {
                return new ServiceResult<List<StatisticModel>>(ServiceResultStatus.NoContent);
            }

            return new ServiceResult<List<StatisticModel>>(ServiceResultStatus.ItemRecieved, statistics.Select(model => mapper.Map<StatisticModel, StatisticInDbModel>(model)).ToList());
        }
    }
}
