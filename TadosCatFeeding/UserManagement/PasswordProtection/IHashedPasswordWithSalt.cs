using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.UserManagement.PasswordProtection
{//????????????????????????????
    public interface IHashedPasswordWithSalt
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
