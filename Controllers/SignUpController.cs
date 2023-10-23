using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IUserRepository _userRepository;

        public SignUpController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel user)
        {
            _userRepository.Create(user);
            return RedirectToAction("Index", "Home");
        }
    }

}
