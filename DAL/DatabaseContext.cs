using BlogASP.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogASP.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions <DatabaseContext> options) : base(options){
            
        }
        public DbSet<UserModel> Users { get; set; }
    }
}
