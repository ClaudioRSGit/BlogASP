using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
