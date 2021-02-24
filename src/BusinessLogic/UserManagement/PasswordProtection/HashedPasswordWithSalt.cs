namespace BusinessLogic.UserManagement.PasswordProtection
{
    public class HashedPasswordWithSalt : IHashedPasswordWithSalt
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
