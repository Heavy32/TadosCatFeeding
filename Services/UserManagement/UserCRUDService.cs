using DataBaseManagement.UserManagement;
using Services.UserManagement.PasswordProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public async Task<ServiceResult<UserGetModel>> GetAsync(int id)
        {
            UserInDbModel userInDb = await database.GetAsync(id);

            if(userInDb == null)
            {
                return new ServiceResult<UserGetModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }

            return new ServiceResult<UserGetModel>(ServiceResultStatus.ItemRecieved, mapper.Map<UserGetModel, UserInDbModel>(userInDb));
        }

        public async Task<ServiceResult<UserServiceModel>> CreateAsync(UserCreateModel info)
        {
            HashedPasswordWithSalt hashSalt = protector.ProtectPassword(info.Password);

            UserInDbModel userInDB = new UserInDbModel(0, info.Login, info.Nickname, (int)info.Role, hashSalt.Salt, hashSalt.Password);

            int userId = await database.CreateAsync(userInDB);
            return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemCreated, new UserServiceModel(userId, info.Login, info.Password, info.Nickname, info.Role));
        }

        public async Task<ServiceResult<UserServiceModel>> UpdateAsync(int id, UserUpdateModel info, IEnumerable<Claim> userClaims)
        {
            int userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
            Roles userRole = (Roles)Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value);

            if (userId != id)
            {
                if (userRole != Roles.Admin)
                {
                    return new ServiceResult<UserServiceModel>(ServiceResultStatus.ActionNotAllowed, "You cannot update this user");
                }
            }

            UserInDbModel user = await database.GetAsync(id);
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

            await database.UpdateAsync(id, newUser);

            return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemChanged);
        }

        public async Task<ServiceResult<UserServiceModel>> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            int userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            Roles userRole = (Roles)Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value);
           
            if(userId != id)
            {
                if(userRole != Roles.Admin)
                {
                    return new ServiceResult<UserServiceModel>(ServiceResultStatus.ActionNotAllowed, "You cannot delete this user");
                }
            }
            
            UserInDbModel user = await database.GetAsync(id);
            if (user == null)
            {
                return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");
            }

            await database.DeleteAsync(id);
            return new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemDeleted);
        }
    }
}
