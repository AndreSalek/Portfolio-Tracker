using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortfolioTracker.Core;
using PortfolioTracker.Core.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PortfolioTracker.Web.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

    }
}
