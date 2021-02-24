using System.Threading.Tasks;

namespace BusinessLogic.UserManagement
{
    public interface IUserEntrance
    {
        public Task<ServiceResult<TokenJwt>> LogInAsync(string login, string password);
    }
}
