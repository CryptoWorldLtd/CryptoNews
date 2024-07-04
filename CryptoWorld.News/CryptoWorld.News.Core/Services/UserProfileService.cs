using Azure.Core;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CryptоWorld.News.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        public UserProfileService(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        public async Task<ApplicationUser> EditProfileAsync(UserProfileModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new ArgumentException("There is no such user.");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Img = model.Img;
            user.Age = model.Age;
            user.Gender = model.Gender.ToString();
            user.PhoneNumber = model.PhoneNumber;
            context.SaveChanges();

            return user;
        }

        public async Task<ApplicationUser> ChangeEmailAsync(ChangeEmailModel model)
        {
            var user = await userManager.FindByEmailAsync(model.CurrentEmail);

            if(user== null)
                throw new ArgumentException("There is no such user with the same email.");

            var areEquel = string.Equals(model.NewEmail, model.ConfirmEmail, StringComparison.OrdinalIgnoreCase);

            if (!areEquel)
                throw new ArgumentException("The New Email and Confirmed email do not match.");

            var token = await userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
            var response = await userManager.ChangeEmailAsync(user, model.NewEmail,token);

            return user;
        }
       
    }
}
