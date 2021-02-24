namespace BusinessLogic.StatisticProvision
{
    public class StatisticModel : IUniqueModel
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string SqlExpression { get; }

        public StatisticModel(int id, string name, string description, string sqlExpression)
        {
            Id = id;
            Name = name;
            Description = description;
            SqlExpression = sqlExpression;
        }
    }
}
