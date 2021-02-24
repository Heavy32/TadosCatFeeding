namespace DataBaseRepositories.StatisticProvision
{
    public class StatisticInDbModel
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string SqlExpression { get; }

        public StatisticInDbModel(int id, string name, string description, string sqlExpression)
        {
            Id = id;
            Name = name;
            Description = description;
            SqlExpression = sqlExpression;
        }
    }
}
