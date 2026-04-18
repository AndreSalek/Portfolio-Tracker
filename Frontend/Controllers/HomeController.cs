using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var isLogged = User.Identity.IsAuthenticated;
            var username = User.Identity.Name;

            ViewBag.IsLogged = isLogged;
            ViewBag.Username = username;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string detailedMessage)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, DetailedMessage = detailedMessage });
        }
    }
}
