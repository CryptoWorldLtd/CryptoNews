using CryptoWorld.News.Core.Contracts;
using CryptoWorld.News.Core.Services;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoWorld.Application.Server.Controllers
{
    [Authorize]
    public class UserProfileController : BaseApiController
    {
        private readonly IUserProfileService profileService;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        public UserProfileController(IUserProfileService _profileService, ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            profileService = _profileService;
            context = _context;
            userManager = _userManager;
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit() 
        {
            var userId = userManager.GetUserId(User);
            var userProfile = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            
            if (userProfile == null)
                return BadRequest(ModelState);
            
            return Ok(userProfile);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] UserProfileModel model)
        {
            try 
            {
                var user = await profileService.EditProfileAsync(model);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }


            return Ok();
        }

        [HttpPost("changeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailModel model)
        {
            try
            {
                var response = await profileService.ChangeEmailAsync(model);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "The user email was changed successfully!" });
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            
            try
            {
                var userId = userManager.GetUserId(User);
                var userProfile = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                var response = await profileService.ChangePasswordAsync(model,userProfile);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "The user password was changed successfully!" });
        }

    }
}
