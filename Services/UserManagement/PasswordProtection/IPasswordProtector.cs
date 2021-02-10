namespace Services.UserManagement.PasswordProtection
{
    public interface IPasswordProtector<TProtectionSchema>
    {
        public TProtectionSchema ProtectPassword(string password);
        public bool VerifyPassword(TProtectionSchema protectedPassword, string password);
    }
}
