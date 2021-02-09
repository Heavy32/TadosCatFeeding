using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TadosCatFeeding.UserManagement
{
    public class UserModel : IUniqueModel
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
        public Roles Role { get; set; }

        public UserModel() { }

        public UserModel(int id, string login, string password, string nickname, Roles role)
        {
            Id = id;
            Login = login;
            Password = password;
            Nickname = nickname;
            Role = role;
        }
    }
}

