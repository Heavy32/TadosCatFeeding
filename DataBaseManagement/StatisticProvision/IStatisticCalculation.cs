using System.Threading.Tasks;

namespace DataBaseManagement.StatisticProvision
{
    public interface IStatisticCalculation
    {
        public Task<StatisticResult> ExecuteAsync(string sqlExpression);
    }
}
