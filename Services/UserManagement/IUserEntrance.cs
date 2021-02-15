using System.Threading.Tasks;

namespace Services.UserManagement
{
    public interface IUserEntrance
    {
        public Task<ServiceResult<TokenJwt>> LogIn(string login, string password);
    }
}
