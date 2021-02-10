using System.ComponentModel.DataAnnotations;

namespace Presentation
{
    public class CatCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
