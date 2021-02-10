using Services.UserManagement;

namespace Presentation.UserManagement
{
    public class UserUpdateViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public Roles Role { get; set; }
    }
}
