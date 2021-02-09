using TadosCatFeeding.UserManagement.PasswordProtection;

namespace TadosCatFeeding.UserManagement
{
    public class UserCRUDService : IUserCRUDService
    {
        private readonly UserRepository database;
        private readonly IPasswordProtector<HashedPasswordWithSalt> protector;

        public UserCRUDService(UserRepository database, IPasswordProtector<HashedPasswordWithSalt> protector)
        {
            this.protector = protector;
            this.database = database;
        }

        public ServiceResult<UserGetModel> Get(int id)
        {
            UserInDB userInDb = database.Get(id);

            if(userInDb == null)
            {
                return new ServiceResult<UserGetModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }
            
            return new ServiceResult<UserGetModel>(ServiceResultStatus.ItemRecieved, database.Map<UserGetModel, UserInDB>(userInDb));
        }

        public ServiceResult<UserModel> Create(UserCreateModel info)
        {
            HashedPasswordWithSalt hashSalt = protector.ProtectPassword(info.Password);

            UserInDB userInDB = new UserInDB(0, info.Login, info.Nickname, (int)info.Role, hashSalt.Salt, hashSalt.Password);

            int userId = database.Create(userInDB);
            return new ServiceResult<UserModel>(ServiceResultStatus.ItemCreated, new UserModel(userId, info.Login, info.Password, info.Nickname, info.Role));
        }

        public ServiceResult<UserModel> Update(int id, UserUpdateModel info)
        {
            UserInDB user = database.Get(id);
            if (user == null)
            {
                return new ServiceResult<UserModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }

            bool IsPasswordSame = protector.VerifyPassword(new HashedPasswordWithSalt { Password = user.HashedPassword, Salt = user.Salt }, info.Password ?? "");
            HashedPasswordWithSalt hashSalt = protector.ProtectPassword(info.Password ?? "");

            UserInDB newUser = new UserInDB(
                id,
                info.Login ?? user.Login,
                info.Nickname ?? user.Nickname,
                info.Role == default ? user.Role : (int)info.Role,
                IsPasswordSame ? user.Salt : hashSalt.Salt,
                IsPasswordSame ? user.HashedPassword : hashSalt.Password
            );

            database.Update(id, newUser);

            return new ServiceResult<UserModel>(ServiceResultStatus.ItemChanged);
        }

        public ServiceResult<UserModel> Delete(int id)
        {
            UserInDB user = database.Get(id);
            if (user == null)
            {
                return new ServiceResult<UserModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }

            database.Delete(id);
            return new ServiceResult<UserModel>(ServiceResultStatus.ItemDeleted);
        }
    }
}
