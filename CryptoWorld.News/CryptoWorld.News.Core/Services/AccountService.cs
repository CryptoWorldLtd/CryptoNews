using CryptoWorld.News.Core.Contracts;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace CryptoWorld.News.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountService(
            UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
        }

        public async Task<IdentityResult>RegisterAsync(RegisterRequestModel model)
        {
            if (!IsValidEmail(model.Email))
            {
                throw new ArgumentException("Invalid email address format.");
            }

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C:\\\\{user}\\\\secrets\\\\secret.json"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailPattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}