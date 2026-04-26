using AspNetCoreGeneratedDocument;
using Frontend.Data;
using Frontend.Data.Models;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Frontend.Controllers
{
    public class UserManagementController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Registration()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Please log in with your credentials.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel userToRegister)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "User was not succesfully registered: " + userToRegister.Username + " " + userToRegister.Admin;
                return View("Registration", userToRegister);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = userToRegister.Username,
                Email = userToRegister.Email
            };

            if (!await _roleManager.RoleExistsAsync(userToRegister.Admin))
            {
                ViewData["Message"] = userToRegister.Admin + " role neexistuje, zadejte existující roli např:" + await _roleManager.RoleExistsAsync(userToRegister.Admin);
                return View("Registration");
            }
            
            var result = await _userManager.CreateAsync(user, userToRegister.Password);
   
            if (!result.Succeeded)
            {
                ViewData["Message"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return View("Registration");
            }
            await _userManager.AddToRoleAsync(user, userToRegister.Admin);
            // await _signInManager.RefreshSignInAsync(user); redundant
            ViewData["Message"] = "User successfully registered: " + userToRegister.Username + userToRegister.Admin;
            return View("Registration");
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserViewModel userToLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userToLogin);
            }

            bool lockoutOnFailure = false;
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(userToLogin.Username, userToLogin.Password, userToLogin.IsLogged, lockoutOnFailure);

            if (result.Succeeded) return RedirectToAction("Index", "Home");

            else
                ModelState.AddModelError("", "Chyba (dodelat zbytek chyb)");
            return View("Login", userToLogin);
        }
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();
             return RedirectToAction("Index", "Home");
        }
    }
}
