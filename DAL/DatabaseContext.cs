    using BlogASP.Models;
    using Microsoft.EntityFrameworkCore;

    namespace BlogASP.DAL
    {
        public class DatabaseContext : DbContext
        {
            public DatabaseContext() { }
            public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
            {

            }
            public DbSet<UserModel> Users { get; set; }
            public DbSet<ArticleModel> Articles { get; set; }
            public DbSet<CommentModel> Comments { get; set; }
            public DbSet<RatingModel> Ratings { get; set; }
        }
    }
