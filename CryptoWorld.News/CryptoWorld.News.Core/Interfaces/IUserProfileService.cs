using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<ApplicationUser> EditProfileAsync(UserProfileModel model);
        Task<IdentityResult> ChangeEmailAsync(ChangeEmailModel model);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model, ApplicationUser userProfile);
        Task LogoutAsync();
        Task<List<ApplicationUser>> GetAllUsersAsync();
    }
}
