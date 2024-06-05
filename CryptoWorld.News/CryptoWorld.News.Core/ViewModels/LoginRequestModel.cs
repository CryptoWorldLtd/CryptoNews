using System.ComponentModel.DataAnnotations;

namespace CryptoWorld.News.Core.ViewModels
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
