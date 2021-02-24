using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLogic.UserManagement
{
    public interface IUserCRUDService
    {
        public Task<ServiceResult<UserGetModel>> GetAsync(int id);
        public Task<ServiceResult<UserServiceModel>> CreateAsync(UserCreateModel info);
        public Task<ServiceResult<UserServiceModel>> UpdateAsync(int id, UserUpdateModel info, IEnumerable<Claim> usersClaim);
        public Task<ServiceResult<UserServiceModel>> DeleteAsync(int id, IEnumerable<Claim> usersClaim);
    }
}
