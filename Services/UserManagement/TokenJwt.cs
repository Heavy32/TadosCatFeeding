using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.UserManagement
{
    public class TokenJwt
    {
        private readonly int tokenDuration;
        private readonly string securityKey;
        private readonly string audience;
        private readonly string issuer;

        public string Token { get; }

        public TokenJwt(ClaimsIdentity identity, int tokenDuration, string securityKey, string audience, string issuer)
        {
            this.tokenDuration = tokenDuration;
            this.securityKey = securityKey;
            this.audience = audience;
            this.issuer = issuer;
            Token = GetToken(identity);
        }

        private string GetToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(tokenDuration)),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                            SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
