using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.UserManagement
{
    public class UserCRUDService
    {
        private readonly UnitOfWork database;

        public UserCRUDService(UnitOfWork database)
        {
            this.database = database;
        }

        public UserModel Get(int id)
        {
            return database.UserRepository.Get(id);
        }

        public int Create(UserModel info)
        {
            return database.UserRepository.Create(info);
        }

        public void Update(int id, UserModel info)
        {
            UserModel user = database.UserRepository.Get(id);

            UserModel newUser = new UserModel
            {
                Id = user.Id,
                Login = user.Login == info.Login ? user.Login : info.Login,
                Password = user.Password == info.Password ? user.Password : info.Password,
                Nickname = user.Nickname == info.Nickname ? user.Nickname : info.Nickname,
                Role = user.Role == info.Role ? user.Role : info.Role,
            };

            database.UserRepository.Update(id, newUser);
        }

        public void Delete(int id)
        {
            database.UserRepository.Delete(id);
        }
    }
}
