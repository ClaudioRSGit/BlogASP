using BlogASP.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BlogASP.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BlogASP.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirecione para a página inicial após o logout
        }

        /////////Login//////////
     
        [HttpPost]
        public IActionResult Login(UserModel loginModel)
        {
            // Encontre o utilizador na base de dados com o mesmo nome de utilizador
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == loginModel.Username);

            if (user != null && user.Password == loginModel.Password)
            {
                // Crie as reivindicações para o utilizador autenticado
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
                // Adicione mais reivindicações conforme necessário
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Faça o login do utilizador
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home"); // Redirecione para a página inicial após o login bem-sucedido
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nome de utilizador ou palavra-passe inválidos");
                return View("Index");
            }
        }
    }
}
