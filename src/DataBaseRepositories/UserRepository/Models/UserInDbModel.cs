namespace DataBaseRepositories.UserManagement
{
    public class UserInDbModel
    {
        public int Id { get; }
        public string Login { get; }
        public string Nickname { get; }
        public int Role { get; }
        public string Salt { get; }
        public string HashedPassword { get; }

        public UserInDbModel(int id, string login, string nickname, int role, string salt, string password)
        {
            Id = id;
            Login = login;
            Nickname = nickname;
            Role = role;
            Salt = salt;
            HashedPassword = password;
        }
    }
}
