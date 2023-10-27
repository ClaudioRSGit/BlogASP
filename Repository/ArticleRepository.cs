using BlogASP.DAL;
using BlogASP.Models;
using Microsoft.AspNetCore.Mvc;
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
            return _databaseContext.Articles.FirstOrDefault(x => x.ArticleId == id);
        }

        public List<ArticleModel> GetAllArticles()
        {
            try
            {
                return _databaseContext.Articles.ToList();
            }
            catch (Exception ex)
            {
                // Registre ou manipule a exceção conforme necessário
                Console.Error.WriteLine($"Error fetching articles: {ex.Message}");
                return null; // Ou outra sinalização de falha
            }
        }

        public ArticleModel CreateArticle(ArticleModel article)
        {
            // Adicione lógica adicional, se necessário, antes de salvar o artigo
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

            if (articleDB == null) throw new Exception("Delete error!");

            // Adicione lógica adicional, se necessário, antes de remover o artigo
            _databaseContext.Articles.Remove(articleDB);
            _databaseContext.SaveChanges();
            return true;
        }

    }
}
