using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Helpers;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.Infrastructure.Services.Interfaces;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using HomeAccounting.WebApi.DTOs.AuthentificationDTOs;
using HomeAccounting.WebApi.DTOs.RegistrationDTOs;
using HomeAccounting.WebApi.DTOs.UserDto;
using HomeAccounting.WebApi.DTOs.WorkingWithPasswordsDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public UsersAccountsController(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IEmailSender emailSender, IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequestDTO userForRegistration)
        {
            var user = _mapper.Map<AppUser>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new UserRegistrationResponseDTO { Errors = errors });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callback = userForRegistration.ClientURI + "?token=" + token + "&email=" + userForRegistration.Email;
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
            {
                user = await _userManager.FindByNameAsync(userForAuthentication.Email);
                if (user == null)
                {
                    return BadRequest("Such user does not exists" );
                }
            }

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = "Invalid Password" });

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = "Email is not confirmed" });

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new UserAuthenticationResponseDTO { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(forgotPasswordDto.Email);
                if (user == null)
                {
                    return BadRequest("Such user does not exists");
                }
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var textToSend = $"Dear User {user.UserName}, " + "\n" + "We got a request from you for reseting the password." + "\n" +
                "Please, follow the next link to reset you passord:" + "\n" + callback;
            var message = new Message(new string[] { user.Email }, "Reseting password", textToSend);
            _emailSender.SendEmail(message);

            return Ok(new ForgotPasswordResponseDto {IsPawwordReseted = true, ResetToken = token });
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
            token = CorrectConfiramtionToken(token);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Such user does not exist");
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid token");
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UserResponseDto>> GetConcreteUser()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            var userDto = _mapper.Map<UserResponseDto>(user);

            if (user == null)
            {
                return BadRequest(new Response<UserResponseDto>
                {
                    Data = null,
                    IsSuccessful = false,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = "Such user does not exist"
                });
            }

            return Ok(new Response<UserResponseDto>
            {
                Data = userDto,
                IsSuccessful = true,
                ErrorCode = null,
                ErrorMessage = null
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("edit")]
        [HttpPost]
        public async Task<ActionResult> EditUSer(UserRequestDto userRequest)
        {
            var userId = User.GetUserId();

            var user = _mapper.Map<AppUser>(userRequest);

            await _userService.EditUser(user, userId);

            return Ok(userRequest);
        }


        public string CorrectConfiramtionToken(string token)
        {
            return token.Replace(" ", "+");
        }
    }
}
