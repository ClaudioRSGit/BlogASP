using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class SelectedArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
