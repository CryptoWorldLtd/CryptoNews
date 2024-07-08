using System.ComponentModel.DataAnnotations;

namespace CryptоWorld.News.Core.ViewModels
{
    public class ForgotPasswordRequestModel
    {
        [Required]
        public string Email { get; set; }
    }
}