using System.ComponentModel.DataAnnotations;


namespace Frontend.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        public required string DisplayName { get; set; }

        public required DateTime DateOfBirth { get; set; }

        public string Admin { get; set; } = "default";

    }
}
