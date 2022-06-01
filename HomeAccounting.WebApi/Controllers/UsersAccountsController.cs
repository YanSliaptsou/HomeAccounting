using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Helpers;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs.AuthentificationDTOs;
using HomeAccounting.WebApi.DTOs.RegistrationDTOs;
using HomeAccounting.WebApi.DTOs.WorkingWithPasswordsDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Route("api/users-accounts")]
    public class UsersAccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        public UsersAccountsController(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequestDTO userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<AppUser>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new UserRegistrationResponseDTO { Errors = errors });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);
            var textToSend = $"Dear User {user.UserName}, " + "\n" + "We got a request from you for confirming your email." + "\n" +
                "Please, follow the next link to confirm your email:" + "\n" + callback;
            var message = new Message(new string[] { user.Email }, "Email Confirmation token", textToSend);
            _emailSender.SendEmail(message);

            return Ok(token);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationRequestDTO userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = "Email is not confirmed" });
            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new UserAuthenticationResponseDTO { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };
            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var textToSend = $"Dear User {user.UserName}, " + "\n" + "We got a request from you for reseting the password." + "\n" +
                "Please, follow the next link to reset you passord:" + "\n" + callback;
            var message = new Message(new string[] { user.Email }, "Reseting password", textToSend);
            _emailSender.SendEmail(message);

            return Ok(token);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok();
        }

        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");
            return Ok();
        }
    }
}
