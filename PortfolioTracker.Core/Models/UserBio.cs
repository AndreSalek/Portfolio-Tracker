using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Models
{
  
    public class UserBio
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
