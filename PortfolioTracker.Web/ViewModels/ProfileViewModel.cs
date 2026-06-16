using System.ComponentModel.DataAnnotations;

namespace PortfolioTracker.Web.ViewModels
{
    public class ProfileViewModel
    {

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }
        
        public  string? DisplayName { get; set; }
        
        public  DateTime? DateOfBirth { get; set; }



    }
}
