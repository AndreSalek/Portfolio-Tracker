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
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        public bool IsLogged { get; set; }
    }
}
