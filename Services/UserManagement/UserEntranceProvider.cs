using DataBaseManagement.UserManagement;
using Services.UserManagement.PasswordProtection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.UserManagement
{
    public class UserEntranceProvider : IUserEntrance
    {
        private readonly IUserRepository database;
        private readonly IPasswordProtector<HashedPasswordWithSalt> protection;
        private readonly IMapper mapper;

        public UserEntranceProvider(IUserRepository database, IPasswordProtector<HashedPasswordWithSalt> protection, IMapper mapper)
        {
            this.database = database;
            this.protection = protection;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<TokenJwt>> LogIn(string login, string password)
        {
            UserInDbModel user = await database.GetUserByLoginAsync(login);
            if (user == null)
            {
                return new ServiceResult<TokenJwt>(ServiceResultStatus.IncorrectLoginPassword);
            }

            if (!protection.VerifyPassword(new HashedPasswordWithSalt { Salt = user.Salt, Password = user.HashedPassword },password))
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
                new Claim(ClaimTypes.NameIdentifier, userClaims.Id.ToString()),
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
