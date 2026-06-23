using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Web.ViewModels;
using System.Diagnostics;

namespace PortfolioTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index() => View();

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var statusCodeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var error = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode ?? 500,
                OriginalPath = statusCodeFeature?.OriginalPath,
            };

            error.Message = statusCode switch
            {
                400 => $"{statusCode}: Invalid request",
                401 => $"{statusCode}: You are not authenticated",
                403 => $"{statusCode}: You do not have access to this resource",
                404 => $"{statusCode}: Page not found",
                _ => "500: Internal Server Error"
            };
            if (exceptionFeature is not null) _logger.LogError(exceptionFeature.Error, exceptionFeature.Error.Message);

            return View(error);
        }
    }
}
