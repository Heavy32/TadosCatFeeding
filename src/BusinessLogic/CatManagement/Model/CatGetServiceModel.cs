namespace BusinessLogic.CatManagement
{
    public class CatGetServiceModel
    {
        public int Id { get; }
        public string Name { get; }
        public int OwnerId { get; }

        public CatGetServiceModel(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
