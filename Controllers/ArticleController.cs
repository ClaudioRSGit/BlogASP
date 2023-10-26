using BlogASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogASP.Controllers
{
    public class ArticleController : Controller
    {
        // GET: /Article
        public IActionResult Index()
        {
            // Logic to get a list of articles
            return View();
        }
    }
}
