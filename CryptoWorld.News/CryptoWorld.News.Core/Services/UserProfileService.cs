using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        public UserProfileService(UserManager<ApplicationUser> _userManager,ApplicationDbContext _context)
        {
            userManager = _userManager;
            context  = _context;
        }
        public async Task<ApplicationUser> EditProfileAsync(UserProfileModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new ArgumentException("There is no such user.");
            
           
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Img = model.Img;
                user.Age = model.Age;
                //user.DateOfBirth = model.BirthDate;
                user.Gender = model.Gender.ToString();
                user.PhoneNumber = model.PhoneNumber;
                context.SaveChanges();
           
            return user;
        }
        
    }
}
