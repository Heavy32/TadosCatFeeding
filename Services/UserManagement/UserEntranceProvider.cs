using DataBaseManagement.UserManagement;
using Services.UserManagement.PasswordProtection;
using System.Collections.Generic;
using System.Security.Claims;

namespace Services.UserManagement
{
    public class UserEntranceProvider : IUserEntrance
    {
        private readonly IUserRepository database;
        private readonly IPasswordProtector<HashedPasswordWithSalt> protection;
        private readonly IMapper mapper;

        public UserEntranceProvider(UserRepository database, IPasswordProtector<HashedPasswordWithSalt> protection)
        {
            this.database = database;
            this.protection = protection;
        }

        public ServiceResult<TokenJwt> LogIn(string login, string password)
        {
            UserInDbModel user = database.GetUserByLogin(login);
            if (user == null)
            {
                return new ServiceResult<TokenJwt>(ServiceResultStatus.IncorrectLoginPassword);
            }

            HashedPasswordWithSalt hashSalt = new HashedPasswordWithSalt
            { 
                Salt = user.Salt,
                Password = user.HashedPassword 
            };

            if (!protection.VerifyPassword(hashSalt, password))
            {
                return new ServiceResult<TokenJwt>(ServiceResultStatus.IncorrectLoginPassword);
            }

            return new ServiceResult<TokenJwt>(ServiceResultStatus.ItemRecieved, 
                new TokenJwt(
                    GetIdentity(mapper.Map<UserClaimsModel, UserInDbModel>(user)),
                    365,
                    "UNBELIEVABLEsecretKEEEEEYYYYYY!!!!!=)",
                    "http://localhost:44338/",
                    "TaskServer"));
        }

        private ClaimsIdentity GetIdentity(UserClaimsModel userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userClaims.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userClaims.Role),
            };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
