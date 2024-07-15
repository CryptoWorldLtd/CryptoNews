using Azure.Core;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Cryptography;
using System.Text;

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

            if (String.IsNullOrWhiteSpace(user.FirstName) ||
                String.IsNullOrWhiteSpace(user.LastName))
                throw new ArgumentException("Invalid user data");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Img = model.Img;
            user.Age = model.Age;
            user.Gender = model.Gender.ToString();
            user.PhoneNumber = model.PhoneNumber;
            context.SaveChanges();

            return user;
        }

        public async Task<IdentityResult> ChangeEmailAsync(ChangeEmailModel model)
        {
            var user = await userManager.FindByEmailAsync(model.CurrentEmail);

            if (user == null)
                throw new ArgumentException("There is no such user with the same email.");

            var areEquel = string.Equals(model.NewEmail, model.ConfirmEmail, StringComparison.OrdinalIgnoreCase);
           
            if (!areEquel)
                throw new ArgumentException("The New Email and Confirmed email do not match.");

            var token = await userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
            var response = await userManager.ChangeEmailAsync(user, model.NewEmail, token);

            return response;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model, ApplicationUser user)
        {
            var currentPassword = user.PasswordHash;
            var areEquel = string.Equals(model.NewPassword, model.ConfirmPassword, StringComparison.OrdinalIgnoreCase);
            if (!areEquel)
                throw new ArgumentException("The New Password and Confirmed password do not match.");

            var verifyPass = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.NewPassword);
            if (verifyPass != PasswordVerificationResult.Success)
                throw new ArgumentException("Can't use the current password as new password.");
            
            var response = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return response;
        }
    }
}
