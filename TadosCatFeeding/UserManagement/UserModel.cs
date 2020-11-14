using System.ComponentModel.DataAnnotations;

namespace TadosCatFeeding.CRUDoperations
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Login is required")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        [StringLength(100, ErrorMessage = "Must be between 5 and 100 characters", MinimumLength = 5)]       
        [EmailAddress]
        public string Login { get; set; }

        [DataType(DataType.Password)]        
        [StringLength(100, ErrorMessage = "Must be between 5 and 100 characters", MinimumLength = 5)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nickname is required")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get;set; }
    }
}

