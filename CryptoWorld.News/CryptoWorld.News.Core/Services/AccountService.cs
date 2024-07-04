using CryptoWorld.News.Core.Contracts;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CryptoWorld.News.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private string secretKey;

        public AccountService(
            UserManager<ApplicationUser> _userManager,
            IConfiguration _config)
        {
            userManager = _userManager;
            config = _config;
            this.secretKey = config["JWT:secretKey"];
        }

        public async Task<IdentityResult>RegisterAsync(RegisterRequestModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            throw new ArgumentException("There is no such user.");

            var passworIsValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!passworIsValid)
            throw new ArgumentException("Password is incorrect.");

            var token = GenerateJwtToken(user);

            return new LoginResponseModel()
            {
                Token = token,
                Email = model.Email,
                Id = user.Id.ToString()
            };
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}