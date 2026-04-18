using AspNetCoreGeneratedDocument;
using Frontend.Data;
using Frontend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Frontend.Controllers
{
    public class UserManagementController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserManagementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Registration()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Login page.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string email, string password, string username)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                ViewData["Message"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return View("Registration");
            }

            ViewData["Message"] = "User successfully registered: " + username;
            return View("Registration");
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(string password, string username)
        {
            bool rememberMe = true;
            bool lockoutOnFailure = false;
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure);

            if (result.Succeeded) return RedirectToAction("Index", "Home");

            return RedirectToAction("Error", "Home");
        }
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();
             return RedirectToAction("Index", "Home");
        }
    }
}
