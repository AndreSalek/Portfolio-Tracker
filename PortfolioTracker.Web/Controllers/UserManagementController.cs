using Frontend.Data;
using Frontend.Data.Models;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Core.Interfaces;
using PortfolioTracker.Core.Services;
using PortfolioTracker.Web.ViewModels;

namespace PortfolioTracker.Web.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UserManagementController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IUserService userService)
        {
            _userManager = userManager;
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var userBio = await _userService.GetUserBioDataAsync(user.Id);
            if (!userBio.Succeeded)
                return RedirectToAction("Error", "Home");

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                DisplayName = userBio.Data.DisplayName,
                DateOfBirth = userBio.Data.DateOfBirth  
            };

            return View("Profile", model);
        }
    }
}
