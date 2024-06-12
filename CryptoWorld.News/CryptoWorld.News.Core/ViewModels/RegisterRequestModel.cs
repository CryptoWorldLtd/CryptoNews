using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CryptoWorld.News.Core.ViewModels
{
    public class RegisterRequestModel
    {
        [Required]
        [MinLength(4, ErrorMessage ="The username should be at least 4 characters long.")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmed password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}