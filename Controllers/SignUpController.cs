using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogASP.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignUpController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            List<string> errors = new List<string>();

            var existingUser = _userRepository.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                errors.Add("Username already exists.Please try another!");
            }

            existingUser = _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                errors.Add("Email already exists.Please try another!");
            }

            if (errors.Count > 0)
            {
                ViewData["CustomErrors"] = errors; 
                return View("Index", user);
            }

            var passwordHash = PasswordHasher.HashPassword(user.Password);
            user.Password = passwordHash;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            _userRepository.Create(user);

            return RedirectToAction("Index", "Home");
        }

    }

}
