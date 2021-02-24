namespace BusinessLogic.UserManagement.PasswordProtection
{
    public interface IHashedPasswordWithSalt
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
