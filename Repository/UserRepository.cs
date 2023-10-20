using BlogASP.DAL;
using BlogASP.Models;

namespace BlogASP.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;
        public UserRepository(DatabaseContext databaseContext) 
        {
            _databaseContext = databaseContext;
        }
        public UserModel Create(UserModel user)
        {
            //Insert on Database
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return user;
        }
    }
}
