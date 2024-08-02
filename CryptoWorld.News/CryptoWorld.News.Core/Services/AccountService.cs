
﻿using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace CryptoWorld.News.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSenderService _emailSenderService;
        private string _secretKey;

        public AccountService(
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IConfiguration config,
                IEmailSenderService emailSenderService
            )
        {
            _userManager = userManager;
            _secretKey = config["JWT:secretKey"];
            _signInManager = signInManager;
            _emailSenderService = emailSenderService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequestModel model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    throw new ArgumentException("User with such email already exists.");

                if (!IsValidEmail(model.Email))
                    throw new ArgumentException("Invalid email address format.");

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));
                string action = "confirmemail";
                var emailBody = GenerateConfirmationLink(action, confirmationToken, model.Email);
                var plainTextContent = $"Dear {model.Username} you can click the link to confirm your action! <a href=\"{emailBody}\" class=\"email-button\">Get Started</a>";
                var htmlContent = $"Dear {model.Username} you can click the link to confirm your action! <a href=\"{emailBody}\" class=\"email-button\">Get Started</a>";

                if (result.Succeeded)
                {
                    await this._signInManager.SignInAsync(user, false);
                    await _emailSenderService.SendEmailAsync(model.Email, model.Username, plainTextContent, htmlContent);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during user registration. {ex}");
                return IdentityResult.Failed(new IdentityError
                {
                    Description =
                    "An error occurred during registration. Please try again later."
                });
            }
        }
        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new ArgumentException("There is no such user.");
                var result = await this._signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (!result.Succeeded)
                    throw new ArgumentException("There was a error while loggin you in! Please try again later or contact an administrator.");

                if (!user.EmailConfirmed)
                    throw new ArgumentException("Email address not confirmed!");

                var token = GenerateJwtToken(user);
                return new LoginResponseModel()
                {
                    Token = token,
                    Email = model.Email,
                    Id = user.Id.ToString()
                };
            }
            catch (ArgumentException ex)
            {
                Log.Warning(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error($"Problem occurred during user login! {ex}");
                throw new InvalidOperationException("An error occurred during login. Please try again later.");
            }
        }
        public async Task<IdentityResult> VerifyEmailAsync(string token, string email)
        {
            try
            {
                if (!IsValidEmail(email))
                    throw new ArgumentException("Invalid email address format.");

            var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    throw new ArgumentException("There is no such user.");

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (!result.Succeeded)
                    throw new ArgumentException("Incorrect email.");

                return (result);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during verification of mail! {ex}");
                throw new Exception($"Error in VerifyEmailAsync {ex}");
            }
        }

        public async Task<IdentityResult> PasswordResetAsync(string token, string email, string newPassword)
        {
            try
            {
                if (!IsValidEmail(email))
                    throw new ArgumentException("Invalid email address format.");

                var user = await _userManager.FindByEmailAsync(email) ?? throw new ArgumentException("There is no such user.");
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!result.Succeeded)
                {
                    throw new ArgumentException("Password reset failed.");
                }

                return (result);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during reset password {ex}");
                throw new Exception($"Error in PasswordResetAsync {ex}");
            }
        }
        public async Task<IdentityResult> GeneratePasswordResetToken(string email)
        {
            try
            {
                if (!IsValidEmail(email))
                    throw new ArgumentException("Invalid email address format.");

                var user = await _userManager.FindByEmailAsync(email) ?? throw new ArgumentException("There is no such user.");
                var resetPassToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(resetPassToken);
                string action = "resetpassword";
                var passwordResetLink = GenerateConfirmationLink(action, encodedToken, email);
                var plainTextContent = $"Dear {user.UserName} you can click the link to confirm your action! <a href=\"{passwordResetLink}\" class=\"email-button\">Get Started</a>";
                var htmlContent = $"Dear {user.UserName} you can click the link to confirm your action! <a href=\"{passwordResetLink}\" class=\"email-button\">Get Started</a>";
                await _emailSenderService.SendEmailAsync(user.Email, user.UserName, plainTextContent, htmlContent);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during generating of password reset token! {ex}");
                return IdentityResult.Failed(new IdentityError
                {
                    Description =
                    "An error occurred during generating of password reset token"
                });
            }
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            try
            {
                var claims = new[]
              {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured during generating Jwt token! {ex}");
                throw new Exception($"Error in GenerateJwtToken {ex}");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;
                var emailPattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

                return Regex.IsMatch(email, emailPattern);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during during validation of mail! {ex}");
                throw new Exception($"Error in IsValidEmail {ex}");
            }
        }

        private string GenerateConfirmationLink(string action, string token, string email)
        {
            try
            {
                var confirmationLink = string.Empty;

                if (action == "resetpassword")
                {
                    confirmationLink = $"https://localhost:5173/{action}/{token}/{email}";
                    return confirmationLink;
                }

                confirmationLink = $"https://localhost:7249/Account/{action}?token={token}&email={email}";
                return confirmationLink;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during generating confirmation link! {ex}");
                throw new Exception($"Error in GenerateConfirmationLink {ex}");
            }
        }
    }
}