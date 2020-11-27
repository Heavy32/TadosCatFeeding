using System.ComponentModel.DataAnnotations;

namespace TadosCatFeeding.CatManagement
{
    public class CatModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pet's name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pet should have an owner!")]
        public int OwnerId { get; set; }
    }
}
