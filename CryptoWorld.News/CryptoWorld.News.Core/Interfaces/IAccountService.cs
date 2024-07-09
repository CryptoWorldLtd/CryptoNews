using CryptoWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CryptoWorld.News.Core.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
        Task<IdentityResult> RegisterAsync(RegisterRequestModel model);
        Task<IdentityResult> VerifyEmailAsync(string token, string email);
        Task<IdentityResult> PasswordResetAsync(string token, string email, string newPassword);
        Task<IdentityResult> GeneratePasswordResetToken(string email);
    }
}