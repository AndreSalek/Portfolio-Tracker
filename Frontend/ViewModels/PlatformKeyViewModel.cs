using BackendLibrary;
using Frontend.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

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
        public string Secret { get; set; } = String.Empty;
        [Display(Name = "Public Key")]
        public string Public { get; set; } = String.Empty;
    }
}
