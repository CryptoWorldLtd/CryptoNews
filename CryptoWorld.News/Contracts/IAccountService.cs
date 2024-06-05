using CryptoWorld.News.Core.ViewModels.Account;

namespace Contracts
{
    public interface IAccountService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
    }
}
