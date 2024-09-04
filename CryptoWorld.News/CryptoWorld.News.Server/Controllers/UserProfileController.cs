using CryptoWorld.News.Core.Interfaces;
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
        private readonly IUserProfileService _profileService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserProfileController(
            IUserProfileService profileService,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _profileService = profileService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit() 
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            
            if (userProfile == null)
                return BadRequest(ModelState);
            
            return Ok(userProfile);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] UserProfileModel model)
        {
            try 
            {
                var user = await _profileService.EditProfileAsync(model);
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
                var response = await _profileService.ChangeEmailAsync(model);
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
                var userId = _userManager.GetUserId(User);
                var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                var response = await _profileService.ChangePasswordAsync(model,userProfile);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "The user password was changed successfully!" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _profileService.LogoutAsync();
            return Ok(new { message = "Successfully logged out" });
        }

        [HttpGet("users")]
        public async Task<List<ApplicationUser>> GetUsers()
        {
                return await _profileService.GetAllUsersAsync();
        }
    }
}
