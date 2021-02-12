using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserManagement
{
    public class UserClaimsModel
    {
        public int Id { get; }
        public string Login { get; }
        public string Role { get; }

        public UserClaimsModel(int id, string login, string role)
        {
            Id = id;
            Login = login;
            Role = role;
        }
    }
}
