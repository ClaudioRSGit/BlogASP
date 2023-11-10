using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BlogASP.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IArticleRepository _articleRepository;

        public AdminPanelController(IUserRepository userRepository, IArticleRepository articleRepository)
        {
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new AdminPanelViewModel
            {
                Users = _userRepository.GetAll(),
                Articles = _articleRepository.GetAllArticles()
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            UserModel user = _userRepository.ListById(id);
            return View(user);
        }

        public IActionResult Show()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel user) 
        {
            _userRepository.Create(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(UserModel user)
        {
            _userRepository.Edit(user);
            return Redirect("/AdminPanel/Users");
        }


        [HttpPost]
        public IActionResult SetAsPrivileged(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && (user.Role == "Public" || user.Role == "Administrator"))
            {
                user.Role = "Privileged";
                _userRepository.Edit(user);

                return Redirect("/AdminPanel/Users");
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult SetAsPrivilegedPending(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && (user.Role == "Public" || user.Role == "Administrator"))
            {
                user.Role = "Privileged";
                _userRepository.Edit(user);

                return Redirect("/AdminPanel/PendingUsers");
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult SetAsPublicUser(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && (user.Role == "Privileged" || user.Role == "Administrator"))
            {
                user.Role = "Public";
                _userRepository.Edit(user);

                return Redirect("/AdminPanel/Users");
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult SetAsAdministrator(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && user.Role == "Privileged")
            {
                user.Role = "Administrator";
                _userRepository.Edit(user);

                return Redirect("/AdminPanel/Users");
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult DisableUser(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null)
            {
                user.Role = "Disabled";
                user.DeletedAt = DateTime.Now;
                _userRepository.Edit(user);
            }

            return Redirect("/AdminPanel/Users");
        }

        [HttpPost]
        public IActionResult DisableUserPending(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null)
            {
                user.Role = "Disabled";
                user.DeletedAt = DateTime.Now;
                _userRepository.Edit(user);
            }

            return Redirect("/AdminPanel/PendingUsers");
        }

        [HttpPost]
        public IActionResult DisableArticle(int id)
        {
            ArticleModel article = _articleRepository.GetArticleById(id);

            if (article != null)
            {
                article.isDisabled = true;
                article.DeletedAt = DateTime.Now;
                _articleRepository.Edit(article);
            }

            return Redirect("/AdminPanel/Articles");
        }

        [HttpPost]
        public IActionResult EnableArticle(int id)
        {
            ArticleModel article = _articleRepository.GetArticleById(id);

            if (article != null)
            {
                article.isDisabled = false;
                article.UpdatedAt = DateTime.Now;
                _articleRepository.Edit(article);
            }
            return Redirect("/AdminPanel/Articles");
        }

        [HttpPost]
        public IActionResult EnableUser(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null)
            {
                user.Role = "Public";
                _userRepository.Edit(user);
            }

            return Redirect("/AdminPanel/Users");
        }

        public IActionResult EditArticle(int id)
        {
            ArticleModel article = _articleRepository.GetArticleById(id);

            if (article == null)
            {
                return View("Error");
            }
            article.Picture = article.Picture;
            return View("EditArticle", article);
        }

        [HttpPost]
        public IActionResult UpdateArticle(ArticleModel article)
        {
            if (ModelState.IsValid)
            {
                _articleRepository.UpdateArticle(article);


                return Redirect("/AdminPanel/Articles");
            }
            return View("EditArticle", article);
        }

        [HttpPost]
        public IActionResult CreateAdmin(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userRepository.GetUserByUsername(user.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username already exists");
                    return View("Create");
                }
                string hashedPassword = PasswordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;

                _userRepository.Create(user);

                return RedirectToAction("Index");
            }
            return View("Create");
        }

        [HttpGet]
        [HttpPost]
        public IActionResult ExportToCSV()
        {
            var users = _userRepository.GetAll();
            var articles = _articleRepository.GetAllArticles(); 

            var csvData = new StringBuilder();
            csvData.AppendLine("UserId,Name,Username,Email,Role,CreatedAt,UpdatedAt,DeletedAt");

            foreach (var user in users)
            {
                csvData.AppendLine($"{user.UserId},{user.Name},{user.Username},{user.Email},{user.Role},{user.CreatedAt},{user.UpdatedAt},{user.DeletedAt}");
            }

            csvData.AppendLine();

            csvData.AppendLine("ArticleId,Title,Category,Stars,isPrivate,isDisabled,DeletedAt");

            foreach (var article in articles)
            {
                csvData.AppendLine($"{article.ArticleId},{article.Title},{article.Category},{article.Stars},{article.isPrivate},{article.isDisabled},{article.DeletedAt}");
            }

            var csvFileName = "exported_data.csv";
            var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());
            return File(csvBytes, "text/csv", csvFileName);
        }

        public IActionResult PendingUsers()
        {
            var viewModel = new AdminPanelViewModel
            {
                Users = _userRepository.GetAll()
            };
            return View(viewModel);
        }

        public IActionResult Users()
        {
            var viewModel = new AdminPanelViewModel
            {
                Users = _userRepository.GetAll()
            };
            return View(viewModel);
        }

        public IActionResult Articles()
        {
            var viewModel = new AdminPanelViewModel
            {
                Articles = _articleRepository.GetAllArticles()
            };
            return View(viewModel);
        }

    }
}
