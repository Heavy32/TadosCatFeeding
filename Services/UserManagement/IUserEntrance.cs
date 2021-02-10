using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserManagement
{
    public interface IUserEntrance
    {
        public ServiceResult<TokenJwt> LogIn(string login, string password);
    }
}
