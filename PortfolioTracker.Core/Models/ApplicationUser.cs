using Microsoft.AspNetCore.Identity;

namespace Frontend.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<PlatformKey> PlatformKeys { get; set; }
    }
}
