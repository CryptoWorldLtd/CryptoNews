using Contracts;
using CryptoWorld.Application.Server.Settings;
using CryptoWorld.News.Core.ViewModels.Account;
using CryptoWorld.News.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly JwtSettings jwtSettings;

        public AccountService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new ArgumentException("UserNotFound");
            }

            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                throw new ArgumentException("IncorrectEmailOrPassword");
            }

            var token = await this.GenerateJwtToken(user);

            //this.users.Update(user);
            //await this.users.SaveChangesAsync();

            return new LoginResponseModel()
            {
                Token = token,
            };
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
            //var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            List<string> roles = (await this.userManager.GetRolesAsync(user)).ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>()
                    {
                        { ClaimTypes.NameIdentifier, user.Id },
                        { ClaimTypes.Name, user.UserName },
                        { ClaimTypes.Email, user.Email },
                    },
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        //new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (string role in roles)
            {
                tokenDescriptor.Claims.Add(ClaimTypes.Role, role);
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}
