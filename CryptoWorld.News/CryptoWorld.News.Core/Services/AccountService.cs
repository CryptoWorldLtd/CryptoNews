using CryptoWorld.News.Core.Contracts;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.Services;
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
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSenderService emailSenderService;
        public AccountService(
            UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, IEmailSenderService _emailSenderService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            emailSenderService = _emailSenderService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequestModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
                throw new ArgumentException("User with such email already exists.");

            if (!IsValidEmail(model.Email))
                throw new ArgumentException("Invalid email address format.");

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            var token = GenerateJwtToken(user);
            var emailBody = GenerateEmailConfirmationLink(model.Email, token);


            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, false);
                await emailSenderService.SendEmailAsync(model.Email, model.Username, emailBody);
            }
            return result;
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new ArgumentException("There is no such user.");

            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
                throw new ArgumentException("There was a error while loggin you in! Please try again later or contact an administrator.");

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
            var secretKey = "abcdefghijklmnopqrstuvwxyzabcdef";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

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

        public string GenerateEmailConfirmationLink(string email, string token)
        {
            var confirmationLink = $"https://localhost:7249/confirmemail?token={token}&email={email}";
            return confirmationLink;
        }

    }
}