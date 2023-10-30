using BlogASP.Models;
using System.Collections.Generic;

namespace BlogASP.Repository
{
    public interface IArticleRepository
    {
        List<ArticleModel> GetRelatedArticles(int articleId);
        List<ArticleModel> GetMostStarredArticles();
        List<ArticleModel> GetArticlesByCategory(string category);
        ArticleModel GetArticleById(int id);
        List<ArticleModel> GetAllArticles();
        ArticleModel CreateArticle(ArticleModel article);
        ArticleModel EditArticle(ArticleModel article);
        bool DeleteArticle(int id);
        void UpdateArticle(ArticleModel article);
    }
}
