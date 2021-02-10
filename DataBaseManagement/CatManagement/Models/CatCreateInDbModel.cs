namespace DataBaseManagement.CatManagement
{
    public class CatCreateInDbModel
    {
        public string Name { get; }
        public int OwnerId { get; }

        public CatCreateInDbModel(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }
    }
}
