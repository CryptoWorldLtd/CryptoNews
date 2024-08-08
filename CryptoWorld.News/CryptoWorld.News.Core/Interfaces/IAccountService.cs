using CryptoWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CryptoWorld.News.Core.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
        Task<IdentityResult> RegisterAsync(RegisterRequestModel model);
        Task<IdentityResult> VerifyEmailAsync(string token, string email);
        Task<IdentityResult> PasswordResetAsync(string token, string email, string newPassword);
        Task<IdentityResult> GeneratePasswordResetToken(string email);
        Task<TokenRequestModel> RefreshTokenAsync(string accessToken, string refreshToken);        
    }
}