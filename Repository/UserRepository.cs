﻿using BlogASP.DAL;
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
            return _databaseContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public List<UserModel> GetAll() 
        {
            return _databaseContext.Users.ToList();
        }
        public UserModel Create(UserModel user)
        {  
            user.Role = "EndUser";
            user.isValidated = false;
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return user;
        }

        public UserModel Edit(UserModel user)
        {
            UserModel userDB = ListById(user.Id);

            if (userDB == null) throw new Exception("Atualization error!");

            userDB.Username = user.Name;
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
