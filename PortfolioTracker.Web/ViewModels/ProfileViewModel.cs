using System.ComponentModel.DataAnnotations;

namespace PortfolioTracker.Web.ViewModels
{
    public class ProfileViewModel
    {   
        public  string? Username { get; set; }
        public  string? Email { get; set; }
        
        public  string? DisplayName { get; set; }
        
        public  DateTime? DateOfBirth { get; set; }



    }
}
