using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Frontend.Controllers
{
    public class UserManagmentController : Controller
    {
        public IActionResult UserManagment()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(string username, string password)
        {
            Console.WriteLine(username + password);
            ViewData["Message"] = "Jsi přihlášen!";
            return View("UserManagment");
        }
    }
}
