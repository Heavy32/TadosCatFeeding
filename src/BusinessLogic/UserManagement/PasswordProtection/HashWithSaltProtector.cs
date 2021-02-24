using System;
using System.Security.Cryptography;

namespace BusinessLogic.UserManagement.PasswordProtection
{
    public class HashWithSaltProtector : IPasswordProtector<HashedPasswordWithSalt>
    {
        private readonly int saltSize;

        public HashWithSaltProtector(int saltSize)
        {
            this.saltSize = saltSize;
        }

        public HashedPasswordWithSalt ProtectPassword(string password)
        {
            var saltBytes = new byte[saltSize];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashedPasswordWithSalt hashSalt = new HashedPasswordWithSalt { Password = hashPassword, Salt = salt };
            return hashSalt;
        }

        public bool VerifyPassword(HashedPasswordWithSalt hashSalt, string enteredPassword)
        {
            var saltBytes = Convert.FromBase64String(hashSalt.Salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == hashSalt.Password;
        }
    }
}
