using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.UserManagement
{
    public class LoginResponseBody
    {
        public string Token { get; }
        public int Id { get; }

        public LoginResponseBody(string token, int id)
        {
            Token = token;
            Id = id;
        }
    }
}
