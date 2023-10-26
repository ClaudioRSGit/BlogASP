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

        public UserModel ListById(int id)
        {
            return _databaseContext.Users.FirstOrDefault(x => x.UserId == id);
        }

        public List<UserModel> GetAll() 
        {
            return _databaseContext.Users.ToList();
        }
        public UserModel Create(UserModel user)
        {  
            user.Role = "EndUser";
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return user;
        }

        public UserModel Edit(UserModel user)
        {
            UserModel userDB = ListById(user.UserId);

            if (userDB == null) throw new Exception("Atualization error!");

            userDB.Username = user.Username;
            userDB.Name = user.Name;
            userDB.Email = user.Email;
            

            _databaseContext.Users.Update(userDB);
            _databaseContext.SaveChanges();

            return userDB;
        }

        public bool Erase(int id)
        {
            UserModel userDB = ListById(id);

            if (userDB == null) throw new Exception("Delete error!");

            _databaseContext.Users.Remove(userDB);
            _databaseContext.SaveChanges();
            return true;
        }
    }
}
