using BlogASP.DAL;
using BlogASP.Migrations;
using BlogASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
                .Include(a => a.Comments) // Include comments
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
            ArticleModel articleDB = GetArticleById(article.ArticleId);

            if (articleDB == null) throw new Exception("Update error!");

            articleDB.Title = article.Title;
            articleDB.Category = article.Category;
            articleDB.Description = article.Description;

            _databaseContext.Articles.Update(articleDB);
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
    }
}
