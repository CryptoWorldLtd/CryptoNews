using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Core.Interfaces;
using Serilog;

namespace CryptoWorld.Application.Server.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)

        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _accountService.RegisterAsync(model);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                else
                {
                    return Ok(new { Message = "The user was registered successfully!" });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error while registerig user! {ex}");
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var token = await _accountService.LoginAsync(model);
                if (token != null)
                {
                    Log.Information("Logging of user is successfully");
                    return Ok(token);
                }
                else
                {
                    Log.Warning("User login failed due to invalid credentials");
                    return BadRequest("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error while logging in user! {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest(new { Message = "Token and email are required." });

            try
            {
                var result = await _accountService.VerifyEmailAsync(token, email);
                if (!result.Succeeded)
                {
                    Log.Warning("The mail is not confirmed!");
                    return BadRequest();
                }
                else
                {
                    Log.Information("The mail is confirmed!");
                    return Redirect("https://localhost:5173/Login");
                }
            }
            catch (Exception ex)
            {

                Log.Error($"Unable to verify email! {ex}");
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest(new { Message = "Email is required." });

            try
            {
                var result = await _accountService.GeneratePasswordResetToken(model.Email);
                if (!result.Succeeded)
                {
                    Log.Warning("Problem with sending of mail for forgotten password");
                    return BadRequest(new { Message = "Oops! Please check your mail again" });
                }
                else
                {
                    Log.Information($"Mail for new password is send to {model.Email}");
                    return Ok(result.Succeeded);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error sending email for changing password! {ex}");
                return BadRequest();
            }

        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(PasswordResetRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest(ModelState);

            try
            {
                var result = await _accountService.PasswordResetAsync(model.Token, model.Email, model.NewPassword);
                if (!result.Succeeded)
                {
                    Log.Warning("Problem with setting of new password!");
                    return BadRequest(new { Message = "Oops, have a problem with new password" });
                }
                else
                {
                    Log.Information("The user sets new password");
                    return Redirect("https://localhost:5173/login");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error setting new password! {ex}");
                return BadRequest();
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> RefreshToken([FromBody] TokenRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest("Invalid refresh token.");
            }

            var response = await _accountService.RefreshTokenAsync(model.RefreshToken);
            if (response == null)
            {
                return BadRequest("Invalid refresh token.");
            }

            return Ok(response);
        }

    }
}