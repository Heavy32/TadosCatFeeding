namespace TadosCatFeeding.UserManagement.PasswordProtection
{
    public class HashedPasswordWithSalt
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
