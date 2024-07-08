using System.ComponentModel.DataAnnotations;

namespace CryptoWorld.News.Core.ViewModels
{
    public class ForgotPasswordRequestModel
    {
        [Required]
        public string Email { get; set; }
    }
}