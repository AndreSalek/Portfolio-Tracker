using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Core.Data;
using PortfolioTracker.Core.Models;
using PortfolioTracker.Core.Services;
using PortfolioTracker.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PortfolioTracker.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Registration()
        {
            

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Please log in with your credentials.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "User was not succesfully registered: " + model.Username + " ";
                return View("Registration", model);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
   
            if (!result.Succeeded)
            {
                ViewData["Message"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return View("Registration");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
            await _emailService.SendConfirmationEmailAsync(user.Email, confirmationLink);
           
            ViewData["Message"] = "User successfully registered: " + model.Username;
            return View("Registration");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool lockoutOnFailure = false;
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.IsLogged, lockoutOnFailure);

            if (result.Succeeded) return RedirectToAction("Index", "Home");

            else
                ModelState.AddModelError("", "Chyba (dodelat zbytek chyb)");
            return View("Login", model);
        }
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();
             return RedirectToAction("Index", "Home");
        }

        
    }
}
