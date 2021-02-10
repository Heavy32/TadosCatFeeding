namespace Services.UserManagement
{
    public interface IUserCRUDService
    {
        public ServiceResult<UserGetModel> Get(int id);
        public ServiceResult<UserServiceModel> Create(UserCreateModel info);
        public ServiceResult<UserServiceModel> Update(int id, UserUpdateModel info);
        public ServiceResult<UserServiceModel> Delete(int id);
    }
}
