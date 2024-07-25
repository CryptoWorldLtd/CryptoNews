using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Core.Interfaces;


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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _accountService.LoginAsync(model);

            return Ok(token);
        }

        [HttpGet("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest(new { Message = "Token and email are required." });

            var result = await _accountService.VerifyEmailAsync(token, email);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Unable to verify email!" });
            }

            return Redirect("https://localhost:5173/Login");
        }

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest(new { Message = "Email is required." });

            var result = await _accountService.GeneratePasswordResetToken(model.Email);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Unable to reset password of this email!" });
            }

            return Ok(result.Succeeded);
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(PasswordResetRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest(ModelState);

            var result = await _accountService.PasswordResetAsync(model.Token, model.Email, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Unable to reset password of this email!" });
            }

            return Redirect("https://localhost:5173/login");
        }
    }
}