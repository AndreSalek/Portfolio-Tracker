using BackendLibrary;
using Frontend.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Frontend.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        public bool IsLogged { get; set; }
    }
}
