using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserManagement
{
    public class UserClaimsModel
    {
        public string Login { get; }
        public string Role { get; }

        public UserClaimsModel(string login, string role)
        {
            Login = login;
            Role = role;
        }
    }
}
