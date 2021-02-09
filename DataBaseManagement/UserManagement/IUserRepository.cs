using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.UserManagement
{
    public interface IUserRepository
    {
        public UserInDB GetUserByLogin(string login);
        public int Create(UserInDB info);
        public void Delete(int id);
        public UserInDB Get(int id);
        public void Update(int id, UserInDB info);
    }
}
