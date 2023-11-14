using BlogASP.DAL;
using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;

namespace BlogASP.Controllers
{
    public class ArticleController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IArticleRepository _articleRepository;
        private readonly IRatingService _ratingService;


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
        public ArticleController(DatabaseContext context, IArticleRepository articleRepository, IRatingService ratingService)
        {
            _context = context;
            _articleRepository = articleRepository;
            _ratingService = ratingService;
        }
        public IActionResult Index()
        {
            var articles = _articleRepository.GetAllArticles();

            var username = User.Identity.Name;
            foreach (var article in articles)
            {
                article.CanEdit = _articleRepository.IsUserAuthor(username, article.ArticleId);
            }

            return View(articles);
        }

        public IActionResult Details(int id)
        {
            var article = _articleRepository.GetArticleById(id);

            if (article == null)
            {
                return NotFound();
            }

            // Pass the repository to the view
            ViewBag.ArticleRepository = _articleRepository;

            return View(article);
        }
        [HttpPost]
        public IActionResult Create(ArticleModel article)
        {
            article.CreatedAt = DateTime.Now;
            article.UpdatedAt = DateTime.Now;

            article.UserName = User.Identity.Name;
            article.isDisabled = false;

            if (ModelState.IsValid)
            {
                article.Stars = 0;
                article.Picture = GetRandomImageLink();


                _articleRepository.CreateArticle(article, User.Identity.Name);

                return RedirectToAction("Details", "Article", new { id = article.ArticleId });
            }

            return View(article);
        }

        [HttpPost]
        public IActionResult ToggleStar(int articleId, bool isStarred)
        {
            try
            {
                var username = User.Identity.Name;

                // Check if the user has starred the article
                var hasStarred = _ratingService.HasStarred(username, articleId);

                if (isStarred && hasStarred)
                {
                    _ratingService.RemoveRating(username, articleId);
                }
                else if (!isStarred && !hasStarred)
                {
                    _ratingService.AddRating(username, articleId);
                }

                var starCount = _ratingService.GetRatingCount(articleId);

                return Json(new { success = true, starCount });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in ToggleStar: {ex.Message}");
                return Json(new { success = false, error = "Internal server error: " + ex.Message });
            }
        }

        private string GetRandomImageLink()
        {
            //obter um link de imagem aleatório
            var imageLinks = new List<string>
            {
                "https://fastly.picsum.photos/id/60/1920/1200.jpg?hmac=fAMNjl4E_sG_WNUjdU39Kald5QAHQMh-_-TsIbbeDNI",
                "https://fastly.picsum.photos/id/96/4752/3168.jpg?hmac=KNXudB1q84CHl2opIFEY4ph12da5JD5GzKzH5SeuRVM",
                "https://fastly.picsum.photos/id/119/3264/2176.jpg?hmac=PYRYBOGQhlUm6wS94EkpN8dTIC7-2GniC3pqOt6CpNU",
                "https://fastly.picsum.photos/id/48/5000/3333.jpg?hmac=y3_1VDNbhii0vM_FN6wxMlvK27vFefflbUSH06z98so",
                "https://fastly.picsum.photos/id/8/5000/3333.jpg?hmac=OeG5ufhPYQBd6Rx1TAldAuF92lhCzAhKQKttGfawWuA",
                "https://fastly.picsum.photos/id/4/5000/3333.jpg?hmac=ghf06FdmgiD0-G4c9DdNM8RnBIN7BO0-ZGEw47khHP4",
                "https://fastly.picsum.photos/id/26/4209/2769.jpg?hmac=vcInmowFvPCyKGtV7Vfh7zWcA_Z0kStrPDW3ppP0iGI",
            };

            Random random = new Random();
            int index = random.Next(imageLinks.Count);

            return imageLinks[index];
        }

        [HttpPost]
        public IActionResult AddComment(int articleId, string commentDescription)
        {
            try
            {
                // Retrieve the article from the database
                ArticleModel article = _articleRepository.GetArticleById(articleId);

                if (article == null)
                {
                    return NotFound("Article not found");
                }

                // Create a new comment
                CommentModel newComment = new CommentModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ArticleId = articleId,
                    UserName = User.Identity.Name,
                    Description = commentDescription,
                };

                // Add the comment to the article
                article.Comments.Add(newComment);

                // Update the article in the database
                _articleRepository.UpdateArticle(article);

                return Ok("Comment added successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate
                Console.WriteLine("Outer Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                return StatusCode(500, "Error adding comment: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ArticleModel article = _articleRepository.GetArticleById(id);

            if (article == null)
            {
                return View("Error");
            }

            return View("Edit", article);
        }

        [HttpPost]
        public IActionResult Update(int id, ArticleModel updatedArticle)
        {
            if (ModelState.IsValid)
            {
                ArticleModel existingArticle = _articleRepository.GetArticleById(id);

                if (existingArticle == null)
                {
                    return NotFound();
                }

                existingArticle.Title = updatedArticle.Title;
                existingArticle.Category = updatedArticle.Category;
                existingArticle.Description = updatedArticle.Description;
                existingArticle.isPrivate = updatedArticle.isPrivate;
                existingArticle.UpdatedAt = DateTime.Now;

                updatedArticle.Stars = existingArticle.Stars;

                _articleRepository.UpdateArticle(existingArticle);

                return RedirectToAction("Details", new { id = existingArticle.ArticleId });
            }

            return View("Edit", updatedArticle);
        }


    }
}
