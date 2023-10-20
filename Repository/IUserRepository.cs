using BlogASP.Models;

namespace BlogASP.Repository
{
    public interface IUserRepository
    {
        UserModel Add(UserModel user);
    }
}
