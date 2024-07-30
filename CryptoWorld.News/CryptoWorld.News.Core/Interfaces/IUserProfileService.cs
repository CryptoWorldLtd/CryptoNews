using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<ApplicationUser> EditProfileAsync(UserProfileModel model);
        Task<IdentityResult> ChangeEmailAsync(ChangeEmailModel model);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model, ApplicationUser userProfile);
        Task LogoutAsync();
    }
}
