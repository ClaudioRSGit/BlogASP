using BlogASP.DAL;
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
                return _databaseContext.Articles.ToList();
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.Error.WriteLine($"Error fetching articles: {ex.Message}");
                return null; // Or another way to signal a failure
            }
        }

        public ArticleModel CreateArticle(ArticleModel article)
        {
            // Add additional logic if needed before saving the article
            _databaseContext.Articles.Add(article);
            _databaseContext.SaveChanges();
            return article;
        }

        public ArticleModel EditArticle(ArticleModel article)
        {
            ArticleModel articleDB = GetArticleById(article.ArticleId);

            if (articleDB == null)
                return null;

            articleDB.Title = article.Title;
            articleDB.Category = article.Category;
            articleDB.Description = article.Description;

            _databaseContext.Articles.Update(articleDB);
            _databaseContext.SaveChanges();

            return articleDB;
        }

        public bool DeleteArticle(int id)
        {
            ArticleModel articleDB = GetArticleById(id);

            if (articleDB == null)
                throw new Exception("Delete error!");

            // Add additional logic if needed before removing the article
            _databaseContext.Articles.Remove(articleDB);
            _databaseContext.SaveChanges();
            return true;
        }
    }
}
