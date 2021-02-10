namespace Services.UserManagement
{
    public class UserUpdateModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public Roles Role { get; set; }
    }
}
