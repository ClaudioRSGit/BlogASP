using BlogASP.DAL;
using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogASP.Controllers
{
    public class ArticleController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IArticleRepository _articleRepository;

        public IActionResult Create()
        {
            return View();
        }
        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var articles = _articleRepository.GetAllArticles();
            return View(articles);
        }

        [HttpPost]
        public IActionResult Create(ArticleModel article)
        {
            if (ModelState.IsValid)
            {
                _articleRepository.CreateArticle(article);
                return RedirectToAction("Index", "Home"); 
            }

            return View(article);
        }
        public IActionResult Details(int id)
        {
            var article = _articleRepository.GetArticleById(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public IActionResult StarArticle(int articleId)
        {
            // Obtenha o artigo do banco de dados
            var article = _articleRepository.GetArticleById(articleId);

            // Atualize o campo Stars
            if (article != null)
            {
                article.Stars += 1;
                _articleRepository.UpdateArticle(article);
            }

            // Retorne o novo número de estrelas para atualização na interface do usuário
            return Json(new { stars = article?.Stars });
        }

        public void UpdateArticle(ArticleModel article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
        }

    }
}
