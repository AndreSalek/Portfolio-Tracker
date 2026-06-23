using AspNetCoreGeneratedDocument;
using Frontend.Data;
using Frontend.Data.Models;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Core.Interfaces;
using PortfolioTracker.Core.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, 
            EmailService emailService, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _userService = userService;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            
            if (!ModelState.IsValid)
                return View("Registration", model);

            var result = await _userService.RegisterAsync(model.Username, model.Email, model.Password, model.DisplayName, model.DateOfBirth);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);
                return View("Registration", model);
            }

            TempData["Email"] = model.Email;
            TempData["Username"] = model.Username;


            return RedirectToAction("SuccessfulRegistration", "Account");

            // // diabled => 
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, user.Email }, Request.Scheme);
            // await _emailService.SendConfirmationEmailAsync(user.Email, confirmationLink);
           
              

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginUser(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

           
            var result = await _userService.LoginAsync(model.Username, model.Password, model.rememberMe);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);
                return View("Login", model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();
             return RedirectToAction("Index", "Home");
        }

        public IActionResult SuccessfulRegistration()
        {
            return View();
        }


    }
}
