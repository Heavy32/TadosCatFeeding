namespace Services.UserManagement
{
    public class UserServiceModel : IUniqueModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public Roles Role { get; set; }

        public UserServiceModel() { }

        public UserServiceModel(int id, string login, string password, string nickname, Roles role)
        {
            Id = id;
            Login = login;
            Password = password;
            Nickname = nickname;
            Role = role;
        }
    }
}

