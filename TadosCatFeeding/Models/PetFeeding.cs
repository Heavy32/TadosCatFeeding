using System.ComponentModel.DataAnnotations;

namespace TadosCatFeeding.Models
{
    public class PetFeeding
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PetId { get; set; }
        [Required]
        public string FeedTime { get; set; }
    }
}
