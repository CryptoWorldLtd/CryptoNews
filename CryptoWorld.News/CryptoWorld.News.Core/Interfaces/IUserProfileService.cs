using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.ViewModels;
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
        Task<ApplicationUser> ChangeEmailAsync(ChangeEmailModel model);
        //Task<ApplicationUser> ChangePasswordAsync(ApplicationUser model);
    }
}
