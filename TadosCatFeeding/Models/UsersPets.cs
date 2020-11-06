using System.ComponentModel.DataAnnotations;

namespace TadosCatFeeding.Models
{
    public class UserPet
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PetId { get; set; }
    }
}
