using System.ComponentModel.DataAnnotations;

namespace Services.CatManagement
{
    public class CatServiceModel : IUniqueModel
    {
        public int Id { get; }
        //[Required(ErrorMessage = "Pet's name is required")]
        public string Name { get; }
        //[Required(ErrorMessage = "Pet should have an owner!")]
        public int OwnerId { get; }

        public CatServiceModel(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
