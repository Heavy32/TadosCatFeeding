namespace DataBaseManagement.CatManagement
{
    public class CatInDbModel
    {
        public int Id { get; }
        public string Name { get; }
        public int OwnerId { get; }

        public CatInDbModel(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
