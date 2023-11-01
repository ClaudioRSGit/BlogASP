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
            user.CreatedAt = DateTime.Now;
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
            userDB.UpdatedAt = DateTime.Now;


            _databaseContext.Users.Update(userDB);
            _databaseContext.SaveChanges();

            return userDB;
        }

        public UserModel GetUserByUsername(string username)
        {
            return _databaseContext.Users.FirstOrDefault(u => u.Username == username);
        }

        public UserModel GetUserByEmail(string email)
        {
            return _databaseContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
