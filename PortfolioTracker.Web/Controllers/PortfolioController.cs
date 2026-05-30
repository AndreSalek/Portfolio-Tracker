using BackendLibrary;
using BackendLibrary.Interfaces;
using Frontend.Data;
using Frontend.Data.Models;
using Frontend.Services;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;
        private readonly KrakenService _krakenService;
        private ILogger<PortfolioController> _logger;
        public PortfolioController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, KrakenService krakenService, ILogger<PortfolioController> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _krakenService = krakenService;
            _logger = logger;
        }

        #region GET METHODS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IPlatformBalances balance =  await _krakenService.GetAccountBalances();
            _logger.LogInformation($"Retrieved balances from Kraken: {String.Join(Environment.NewLine, balance)}");

            return View(nameof(Index), balance);
        }

        [HttpGet]
        public async Task<IActionResult> KeyManagement()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? throw new KeyNotFoundException("Missing user ID claim.");

            user.PlatformKeys = await _dbContext.PlatformKeys.Where(key => key.UserId == user.Id).ToListAsync();
            IEnumerable<PlatformKeyViewModel> keyViewModels = user.PlatformKeys.Select(key => new PlatformKeyViewModel
            {
                Id = key.Id.ToString(),
                Platform = key.Platform,
                Secret = key.Secret,
                Public = key.Public
            });
            keyViewModels ??= new List<PlatformKeyViewModel>();

            return View(keyViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> AddPlatformKey()
        {
            return View(new PlatformKeyViewModel());
        }

        #endregion

        #region POST METHODS
        [HttpPost]
        public async Task<IActionResult> AddPlatformKey(PlatformKeyViewModel model)
        {
            model.Id = Guid.NewGuid().ToString();

            // Revalidate model due to ID being generated server-side
            ModelState.Clear();
            if (!TryValidateModel(model))
            {
                // TODO: Replace with proper logging
                OutputModelStateErrors(ModelState);
                return View(model);
            }
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? throw new KeyNotFoundException("Missing user ID claim.");

            PlatformKey key = new PlatformKey
            {
                UserId = user.Id,
                Platform = model.Platform,
                Secret = model.Secret,
                Public = model.Public
            };

            await _dbContext.PlatformKeys.AddAsync(key);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(KeyManagement));
        }

        public async Task<IActionResult> RemovePlatformKey(PlatformKeyViewModel model)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? throw new KeyNotFoundException("Missing user ID claim.");

            PlatformKey platformKey = _dbContext.PlatformKeys.Where(key => (key.Id == model.Id) && (user.Id == key.UserId)).SingleOrDefault() 
                ?? throw new KeyNotFoundException("PlatformKey does not exist");

            _dbContext.Remove(platformKey);
            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction(nameof(KeyManagement));
        }
        #endregion

        private void OutputModelStateErrors(ModelStateDictionary modelState)
        {
            var allErrors = modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            Console.WriteLine("Model state is invalid. Errors:");
            foreach (var error in allErrors)
            {
                Console.WriteLine(error);
            }
        }
    }
}
