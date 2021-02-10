namespace Services.CatManagement
{
    public class CatCreateServiceModel
    {
        public string Name { get; }
        public int OwnerId { get; }

        public CatCreateServiceModel(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }
    }
}
