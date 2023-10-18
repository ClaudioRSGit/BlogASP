using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
