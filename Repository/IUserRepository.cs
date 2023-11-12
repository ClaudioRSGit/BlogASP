using BlogASP.Models;

namespace BlogASP.Repository
{
    public interface IUserRepository
    {
        UserModel ListById(int id);
        List<UserModel> GetAll();
        UserModel Create(UserModel user);
        UserModel Edit(UserModel user);
        UserModel GetUserByUsername(string username);
        UserModel GetUserByEmail(string email);
        public bool APIDeleteUser(int userId);
    }
}
