using BlogASP.DAL;
using BlogASP.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlogASP.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CommentRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<CommentModel> GetCommentsForArticle(int articleId)
        {
            return _databaseContext.Comments
                .Where(c => c.ArticleId == articleId)
                .ToList();
        }

        public CommentModel AddCommentToArticle(int articleId, int userId, CommentModel comment)
        {
            comment.CreatedAt = DateTime.Now;
            comment.UpdatedAt = DateTime.Now;
            comment.ArticleId = articleId;
            comment.UserId = userId; // Set the user ID
            _databaseContext.Comments.Add(comment);
            _databaseContext.SaveChanges();

            return comment;
        }
    }
}