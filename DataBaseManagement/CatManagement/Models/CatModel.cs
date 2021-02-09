using System.ComponentModel.DataAnnotations;

namespace TadosCatFeeding.CatManagement
{
    public class CatModel : IUniqueModel
    {
        public int Id { get; }
        [Required(ErrorMessage = "Pet's name is required")]
        public string Name { get; }
        [Required(ErrorMessage = "Pet should have an owner!")]
        public int OwnerId { get; }

        public CatModel(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
