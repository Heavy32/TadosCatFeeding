using TadosCatFeeding.Abstractions;

namespace TadosCatFeeding.UserManagement
{
    public interface IUserRepository : IRepository<UserModel>
    {
        UserModel GetUserByLogindAndPassword(string login, string password);
    }
}
