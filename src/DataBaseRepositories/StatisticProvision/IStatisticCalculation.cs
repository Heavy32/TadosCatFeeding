using System.Threading.Tasks;

namespace DataBaseRepositories.StatisticProvision
{
    public interface IStatisticCalculation
    {
        public Task<StatisticResult> ExecuteAsync(string sqlExpression);
    }
}
