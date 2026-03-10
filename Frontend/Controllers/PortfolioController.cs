using Frontend.Data;
using Frontend.Data.Models;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;

        public PortfolioController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        #region GET METHODS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new KeyNotFoundException("Missing user ID claim.");

            IEnumerable<PlatformKey> keyList = user.PlatformKeys ?? new List<PlatformKey>();
            IEnumerable<PlatformKeyViewModel> keyViewModels = keyList.Select(key => new PlatformKeyViewModel
            {
                Id = key.Id.ToString(),
                Platform = key.Platform,
                SecretKey = key.SecretKey
            });
   
            return View(keyViewModels);
        }
        #endregion

    }
}
