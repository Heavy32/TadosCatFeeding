using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.Abstractions;

namespace TadosCatFeeding.UserManagement
{
    public interface IUserRepository : IRepository<UserModel>
    {
        UserModel GetUserByLogindAndPassword(string login, string password);
    }
}
