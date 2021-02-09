using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.UserManagement
{
    public class UserUpdateModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public Roles Role { get; set; }
    }
}
