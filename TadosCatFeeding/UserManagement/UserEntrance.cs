using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.UserManagement.PasswordProtection;

namespace TadosCatFeeding.UserManagement
{
    //change the name
    public class UserEntrance : IUserEntrance
    {
        private readonly UserRepository database;
        private readonly IPasswordProtector<HashedPasswordWithSalt> protection;

        public UserEntrance(UserRepository database, IPasswordProtector<HashedPasswordWithSalt> protection)
        {
            this.database = database;
            this.protection = protection;
        }

        public ServiceResult<TokenJwt> LogIn(string login, string password)
        {
            UserInDB user = database.GetUserByLogin(login);
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
                    GetIdentity(database.Map<UserClaimsModel, UserInDB>(user)),
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

        //private object GetToken(ClaimsIdentity identity)
        //{
        //    var now = DateTime.UtcNow;

        //    var jwt = new JwtSecurityToken(
        //            issuer: "TaskServer",
        //            audience: "http://localhost:44338/",
        //            notBefore: now,
        //            claims: identity.Claims,
        //            expires: now.Add(TimeSpan.FromDays(365)),
        //            signingCredentials: new SigningCredentials(
        //                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("supersecret_supersavekey!666")),
        //                    SecurityAlgorithms.HmacSha256));

        //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        //    return encodedJwt;
        //}
    }
}
