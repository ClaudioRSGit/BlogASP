using BlogASP.Models;

namespace BlogASP.Repository
{
    public interface IUserRepository
    {
        List<UserModel> GetAll();
        UserModel Create(UserModel user);
    }
}
