using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.UserManagement
{
    public interface IUserRepository
    {
        public UserInDbModel GetUserByLogin(string login);
        public int Create(UserInDbModel info);
        public void Delete(int id);
        public UserInDbModel Get(int id);
        public void Update(int id, UserInDbModel info);
    }
}
