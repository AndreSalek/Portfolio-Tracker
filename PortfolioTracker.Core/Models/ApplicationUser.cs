using Microsoft.AspNetCore.Identity;

namespace PortfolioTracker.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<PlatformKey> PlatformKeys { get; set; }
    }
}
