using DataBaseManagement.UserManagement;
using Services.UserManagement.PasswordProtection;

namespace Services.UserManagement
{
    public class UserCRUDService : IUserCRUDService
    {
        private readonly IUserRepository database;
        private readonly IPasswordProtector<HashedPasswordWithSalt> protector;
        private readonly IMapper mapper;

        public UserCRUDService(IUserRepository database, IPasswordProtector<HashedPasswordWithSalt> protector, IMapper mapper)
        {
            this.database = database;
            this.protector = protector;
            this.mapper = mapper;
        }

        public ServiceResult<UserGetModel> Get(int id)
        {
            UserInDbModel userInDb = database.Get(id);

            if(userInDb == null)
            {
                return new ServiceResult<UserGetModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }
            
            return new ServiceResult<UserGetModel>(ServiceResultStatus.ItemRecieved, mapper.Map<UserGetModel, UserInDbModel>(userInDb));
        }

        public ServiceResult<UserServiceModel> Create(UserCreateModel info)
        {
            HashedPasswordWithSalt hashSalt = protector.ProtectPassword(info.Password);

            UserInDbModel userInDB = new UserInDbModel(0, info.Login, info.Nickname, (int)info.Role, hashSalt.Salt, hashSalt.Password);

            int userId = database.Create(userInDB);
            return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemCreated, new UserServiceModel(userId, info.Login, info.Password, info.Nickname, info.Role));
        }

        public ServiceResult<UserServiceModel> Update(int id, UserUpdateModel info)
        {
            UserInDbModel user = database.Get(id);
            if (user == null)
            {
                return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }

            bool IsPasswordSame = protector.VerifyPassword(new HashedPasswordWithSalt { Password = user.HashedPassword, Salt = user.Salt }, info.Password ?? "");
            HashedPasswordWithSalt hashSalt = protector.ProtectPassword(info.Password ?? "");

            UserInDbModel newUser = new UserInDbModel(
                id,
                info.Login ?? user.Login,
                info.Nickname ?? user.Nickname,
                info.Role == default ? user.Role : (int)info.Role,
                IsPasswordSame ? user.Salt : hashSalt.Salt,
                IsPasswordSame ? user.HashedPassword : hashSalt.Password
            );

            database.Update(id, newUser);

            return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemChanged);
        }

        public ServiceResult<UserServiceModel> Delete(int id)
        {
            UserInDbModel user = database.Get(id);
            if (user == null)
            {
                return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }

            database.Delete(id);
            return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemDeleted);
        }
    }
}
