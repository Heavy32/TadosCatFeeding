using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CatCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
