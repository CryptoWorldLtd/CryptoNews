using System.ComponentModel.DataAnnotations;

namespace CryptoWorld.Application.Server.Models
{
    public class LoginRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
