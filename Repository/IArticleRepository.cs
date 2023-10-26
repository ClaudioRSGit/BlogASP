using BlogASP.Models;
using System.Collections.Generic;

namespace BlogASP.Repository
{
    public interface IArticleRepository
    {
        ArticleModel GetArticleById(int id);
        List<ArticleModel> GetAllArticles();
        ArticleModel CreateArticle(ArticleModel article);
        ArticleModel EditArticle(ArticleModel article);
        bool DeleteArticle(int id);
        void UpdateArticle(ArticleModel article);
    }
}
