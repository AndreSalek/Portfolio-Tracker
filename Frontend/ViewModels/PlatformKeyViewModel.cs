using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using BackendLibrary;

namespace Frontend.ViewModels
{
    public class PlatformKeyViewModel
    {
        [Required]
        public string Id { get; set; }
        [TypeConverter(typeof(StringToEnumConverter<Platform>))]
        [Required]
        public Platform Platform { get; set; }
        [Required]
        [Display(Name = "Secret Key")]
        public string SecretKey { get; set; } = String.Empty;
    }
}
