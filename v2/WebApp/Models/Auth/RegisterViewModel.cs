using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Auth
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
