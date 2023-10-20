using BlogASP.Models;

namespace BlogASP.Repository
{
    public interface IUserRepository
    {
        UserModel Create(UserModel user);
    }
}
