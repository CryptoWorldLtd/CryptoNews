using CryptoNews.World.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CryptoNews.World.Core.Contracts
{
    public interface IAccountService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
        Task<IdentityResult> RegisterAsync(RegisterRequestModel model);
    }
}
