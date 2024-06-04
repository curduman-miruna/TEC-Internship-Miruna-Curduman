using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Auth
{
    public class LoginViewModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
