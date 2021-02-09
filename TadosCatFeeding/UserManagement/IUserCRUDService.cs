namespace TadosCatFeeding.UserManagement
{
    public interface IUserCRUDService
    {
        public ServiceResult<UserGetModel> Get(int id);
        public ServiceResult<UserModel> Create(UserCreateModel info);
        public ServiceResult<UserModel> Update(int id, UserUpdateModel info);
        public ServiceResult<UserModel> Delete(int id);
    }
}
