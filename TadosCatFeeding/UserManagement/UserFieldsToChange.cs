using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.UserManagement
{
    public class UserFieldsToChange
    {
        public int Id { get; set; }
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        [EmailAddress]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Must be between 5 and 100 characters", MinimumLength = 5)]
        public string Password { get; set; }

        public string Nickname { get; set; }
        public string Role { get; set; }
    }
}
