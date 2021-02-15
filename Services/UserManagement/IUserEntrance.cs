using System.Threading.Tasks;

namespace Services.UserManagement
{
    public interface IUserEntrance
    {
        public Task<ServiceResult<TokenJwt>> LogInAsync(string login, string password);
    }
}
