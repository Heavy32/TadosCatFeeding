using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.UserManagement
{
    public interface IUserRepository
    {
        public Task<UserInDbModel> GetUserByLoginAsync(string login);
        public Task<int> CreateAsync(UserInDbModel info);
        public Task DeleteAsync(int id);
        public Task<UserInDbModel> GetAsync(int id);
        public Task UpdateAsync(int id, UserInDbModel info);
    }
}
