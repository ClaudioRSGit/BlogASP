using BlogASP.Models;
using System.Collections.Generic;

namespace BlogASP.Repositories
{
    public interface ICommentRepository
    {
        List<CommentModel> GetCommentsForArticle(int articleId);
        CommentModel AddCommentToArticle(int articleId, int userId, CommentModel comment);
    }
}