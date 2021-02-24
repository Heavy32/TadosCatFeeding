namespace BusinessLogic.CatManagement
{
    public class CatServiceModel : IUniqueModel
    {
        public int Id { get; }
        public string Name { get; }
        public int OwnerId { get; }

        public CatServiceModel(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
