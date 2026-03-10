using AspNetCoreGeneratedDocument;
using Frontend.Data;
using Frontend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> ManageAdminUser()
        {
            if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Portfolio");

            return View(nameof(ManageAdminUser));
        }

        // TODO: DELETE THIS REGION AFTER FULL IMPLEMENTATION
        #region Admin User
        [HttpPost]
        public async Task<IActionResult> CreateAdminUser()
        {
            ApplicationUser user = new ApplicationUser();
            var data = new 
            {
                UserName = "admin",
                Email = "admin@saman.com",
                Password = "Admin00#"
            };

            await _userManager.SetUserNameAsync(user, data.UserName);
            await _userManager.SetEmailAsync(user, data.Email);
            await _userManager.AddPasswordAsync(user, data.Password);

            var result = await _userManager.CreateAsync(user, data.Password);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdminUser()
        {
            var data = new 
            {
                UserName = "admin",
                Password = "Admin00#"
            };
            bool rememberMe = true;
            bool lockoutOnFailure = false;
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(data.UserName, data.Password, rememberMe, lockoutOnFailure);

            if (result.Succeeded) return RedirectToAction("Index", "Portfolio");
            
            return RedirectToAction("Error", "Home");
        }
        #endregion
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
