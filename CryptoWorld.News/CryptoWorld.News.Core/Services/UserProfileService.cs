using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Extension;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CryptоWorld.News.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        
        public UserProfileService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public async Task<ApplicationUser> EditProfileAsync(UserProfileModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

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
            _context.SaveChanges();

            return user;
        }

        public async Task<IdentityResult> ChangeEmailAsync(ChangeEmailModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.CurrentEmail);

            if (user == null)
                throw new ArgumentException("There is no such user with the same email.");

            var areEquel = string.Equals(model.NewEmail, model.ConfirmEmail, StringComparison.OrdinalIgnoreCase);
           
            if (!areEquel)
                throw new ArgumentException("The New Email and Confirmed email do not match.");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
            var response = await _userManager.ChangeEmailAsync(user, model.NewEmail, token);

            return response;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model, ApplicationUser user)
        {
            var currentPassword = user.PasswordHash;
            var areEquel = string.Equals(model.NewPassword, model.ConfirmPassword, StringComparison.OrdinalIgnoreCase);
            if (!areEquel)
                throw new ArgumentException("The New Password and Confirmed password do not match.");

            var verifyPass = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.NewPassword);
            if (verifyPass != PasswordVerificationResult.Success)
                throw new ArgumentException("Can't use the current password as new password.");
            
            var response = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return response;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
            
        }
    }
}
