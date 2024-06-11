using System.ComponentModel.DataAnnotations;

namespace CryptoWorld.News.Core.ViewModels
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}