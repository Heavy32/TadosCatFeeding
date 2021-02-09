using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.UserManagement.PasswordProtection
{
    public interface IPasswordProtector<TProtectionSchema>
    {
        public TProtectionSchema ProtectPassword(string password);
        public bool VerifyPassword(TProtectionSchema protectedPassword, string password);
    }
}
