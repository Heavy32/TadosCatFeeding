using System.Collections.Generic;
using System.Security.Claims;

namespace Services.UserManagement
{
    public interface IUserCRUDService
    {
        public ServiceResult<UserGetModel> Get(int id);
        public ServiceResult<UserServiceModel> Create(UserCreateModel info);
        public ServiceResult<UserServiceModel> Update(int id, UserUpdateModel info, IEnumerable<Claim> usersClaim);
        public ServiceResult<UserServiceModel> Delete(int id, IEnumerable<Claim> usersClaim);
    }
}
