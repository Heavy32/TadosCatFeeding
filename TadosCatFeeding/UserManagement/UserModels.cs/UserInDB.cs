using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.UserManagement.PasswordProtection;

namespace TadosCatFeeding.UserManagement
{
    public class UserInDB : IUniqueModel
    {
        public int Id { get; }
        public string Login { get; }
        public string Nickname { get; }
        public int Role { get; }
        public string Salt { get; }
        public string HashedPassword { get; }

        public UserInDB(int id, string login, string nickname, int role, string salt, string password)
        {
            Id = id;
            Login = login;
            Nickname = nickname;
            Role = role;
            Salt = salt;
            HashedPassword = password;
        }
    }
}
