using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Show()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
