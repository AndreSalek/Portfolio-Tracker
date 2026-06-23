using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortfolioTracker.Core;
using PortfolioTracker.Core.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PortfolioTracker.Web.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        public bool rememberMe { get; set; }
    }
}
