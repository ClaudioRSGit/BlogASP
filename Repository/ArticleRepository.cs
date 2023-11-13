using BlogASP.DAL;
using BlogASP.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogASP.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ArticleRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<ArticleModel> GetRelatedArticles(int articleId)
        {
            var targetArticle = _databaseContext.Articles.Find(articleId);
            if (targetArticle == null)
            {
                return new List<ArticleModel>(); // or handle as appropriate
            }

            return _databaseContext.Articles
                .Where(a => a.Category == targetArticle.Category && a.ArticleId != articleId)
                .ToList();
        }

        public List<ArticleModel> GetMostStarredArticles()
        {
            return _databaseContext.Articles
                .OrderByDescending(a => a.Stars)
                .Take(3)
                .ToList();
        }

        public List<ArticleModel> GetArticlesByCategory(string category)
        {
            return _databaseContext.Articles.Where(a => a.Category == category).ToList();
        }

        public void UpdateArticle(ArticleModel article)
        {
            article.UpdatedAt = DateTime.Now;
            _databaseContext.Articles.Update(article);
            _databaseContext.SaveChanges();
        }

        public ArticleModel GetArticleById(int id)
        {
            return _databaseContext.Articles
                .Include(a => a.Comments)
                .FirstOrDefault(x => x.ArticleId == id);
        }

        public List<ArticleModel> GetAllArticles()
        {
            try
            {
                var articles = _databaseContext.Articles
                    .Include(a => a.User)
                    .ToList();
                return articles;
            }
            catch (Exception ex)
            {
                // Log the exception using a proper logging framework
                Console.Error.WriteLine($"Error fetching articles: {ex.Message}");
                throw; // Re-throw the exception after logging
            }
        }

        public ArticleModel CreateArticle(ArticleModel article, string userName)
        {
            article.isDisabled = false;
            article.CreatedAt = DateTime.Now;
            article.UpdatedAt = DateTime.Now;
            article.UserName = userName;
            _databaseContext.Articles.Add(article);
            _databaseContext.SaveChanges();
            return article;
        }

        public ArticleModel Edit(ArticleModel article)
        {
            // Fetch the existing article including the Stars property
            ArticleModel articleDB = _databaseContext.Articles
                .FirstOrDefault(x => x.ArticleId == article.ArticleId);

            if (articleDB == null) throw new Exception("Update error!");

            // Store the existing stars in a temporary variable
            int? tempStars = articleDB.Stars;

            if (!string.IsNullOrEmpty(article.Title))
            {
                articleDB.Title = article.Title;
            }
            if (!string.IsNullOrEmpty(article.Category))
            {
                articleDB.Category = article.Category;
            }
            if (!string.IsNullOrEmpty(article.Description))
            {
                articleDB.Description = article.Description;
            }
            if (!string.IsNullOrEmpty(article.Picture))
            {
                articleDB.Picture = article.Picture;
            }

            // Reassign the existing stars back to the articleDB
            articleDB.Stars = tempStars;

            articleDB.isDisabled = false;
            articleDB.UpdatedAt = DateTime.Now;
            articleDB.UserName = article.UserName;

            // Update the entire article
            _databaseContext.Update(articleDB);
            _databaseContext.SaveChanges();

            return articleDB;
        }


        public bool DeleteArticle(int id)
        {
            var article = _databaseContext.Articles.Find(id);
            article.DeletedAt = DateTime.Now;
            ArticleModel articleDB = GetArticleById(id);
            if (articleDB == null)
                throw new Exception("Delete error!");

            // Add additional logic if needed before removing the article
            _databaseContext.Articles.Remove(articleDB);
            _databaseContext.SaveChanges();
            return true;
        }
        public bool IsUserAuthor(string username, int articleId)
        {
            var article = _databaseContext.Articles.FirstOrDefault(a => a.ArticleId == articleId && a.User.Username == username);
            return article != null;
        }
        public ArticleModel APICreateArticle(ArticleModel article, string userName)
        {
            article.isDisabled = false;
            article.CreatedAt = DateTime.Now;
            article.UpdatedAt = DateTime.Now;
            article.UserName = userName;
            _databaseContext.Articles.Add(article);
            _databaseContext.SaveChanges();
            return article;
        }
        public ArticleModel APIEditArticle(int id, ArticleModel newData, string userName)
        {
            var existingArticle = GetArticleById(id);

            if (existingArticle == null)
            {
                throw new Exception("Article not found");
            }

            existingArticle.Title = newData.Title;
            existingArticle.Category = newData.Category;
            existingArticle.Description = newData.Description;
            existingArticle.UpdatedAt = DateTime.Now;
            existingArticle.Picture = newData.Picture;

            _databaseContext.SaveChanges();

            return existingArticle;
        }
    }
}
