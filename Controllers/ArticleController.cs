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

        public List<ArticleModel> GetArticlesByCategory(string category)
        {
            try
            {
                var articles = _context.Articles.Where(a => a.Category == category).ToList();
                return articles;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return null;
            }
        }
        public ArticleController(DatabaseContext context, IArticleRepository articleRepository)
        {
            _context = context;
            _articleRepository = articleRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var articles = _articleRepository.GetAllArticles();
            return View(articles);
        }

        public IActionResult Details(int id)
        {
            // Lógica para obter e exibir os detalhes do artigo
            var article = _articleRepository.GetArticleById(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
        [HttpPost]
        public IActionResult Create(ArticleModel article)
        {
            if (ModelState.IsValid)
            {
                article.Stars = 0; // Defina o número inicial de estrelas aqui
                _articleRepository.CreateArticle(article);
                return RedirectToAction("Index", "Home");
            }

            return View(article);
        }
        [HttpPost]
        public IActionResult StarArticle(int articleId)
        {
            var article = _articleRepository.GetArticleById(articleId);

            if (article != null)
            {
                // Verificar se o usuário já deu estrela (usando cookies como exemplo)
                bool userHasStarred = HttpContext.Request.Cookies["userHasStarred-" + articleId] == "true";

                // Adicionar ou remover estrela com base na verificação
                article.Stars += userHasStarred ? -1 : 1;

                // Atualizar o status de estrela do usuário (usando cookies como exemplo)
                HttpContext.Response.Cookies.Append("userHasStarred-" + articleId, (!userHasStarred).ToString());

                _articleRepository.UpdateArticle(article);
            }

            return Json(new { stars = article?.Stars });
        }


        public void UpdateArticle(ArticleModel article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
        }

    }
}
