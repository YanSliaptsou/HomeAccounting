using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services;
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
        private readonly IUserService _userService;
        private readonly IEmailBilder _emailBilder;
        private readonly IQueryParamsService _queryParamsService;

        private const string ERROR_USER_DOES_NOT_EXIST = "Such user does not exists";
        private const string ERROR_INVALID_PASSWORD = "Invalid Password";
        private const string ERROR_EMAIL_IS_NOT_CONFIRMED = "Email is not confirmed";
        private const string ERROR_INVALID_TOKEN = "Invalid token";
        public UsersAccountsController(
            UserManager<AppUser> userManager, 
            IMapper mapper, 
            ITokenService tokenService, 
            IUserService userService, 
            IEmailBilder emailBilder, 
            IQueryParamsService queryParamsService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _userService = userService;
            _emailBilder = emailBilder;
            _queryParamsService = queryParamsService;
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
            var callback = _queryParamsService.BuildQueryString(userForRegistration.ClientURI, token, userForRegistration.Email);
            await _emailBilder.GenerateEmailMessage(Domain.Enums.EmailMessageTemplate.ConfirmEmail, callback, user, userForRegistration.Password);

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
                    return BadRequest(new Response<UserAuthenticationRequestDTO>
                    {
                        Data = null,
                        ErrorCode = HttpStatusCode.BadRequest.ToString(),
                        ErrorMessage = ERROR_USER_DOES_NOT_EXIST,
                        IsSuccessful = false
                    });
                }
            }

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = ERROR_INVALID_PASSWORD });

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = ERROR_EMAIL_IS_NOT_CONFIRMED });

            var token = _tokenService.GetToken(user);
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
                    return BadRequest(new Response<UserAuthenticationRequestDTO>
                    {
                        Data = null,
                        ErrorCode = HttpStatusCode.BadRequest.ToString(),
                        ErrorMessage = ERROR_USER_DOES_NOT_EXIST,
                        IsSuccessful = false
                    });
                }
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callback = _queryParamsService.BuildQueryString(forgotPasswordDto.ClientURI, token, user.Email);
            await _emailBilder.GenerateEmailMessage(Domain.Enums.EmailMessageTemplate.ResetPassword, callback, user, null);
            return Ok(new ForgotPasswordResponseDto {IsPawwordReseted = true, ResetToken = token });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            resetPasswordDto.Token = CorrectConfiramtionToken(resetPasswordDto.Token);
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok(new Response<AppUser> { 
                Data = user,
                IsSuccessful = true,
                ErrorMessage = null,
                ErrorCode = null
            });
        }

        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            token = CorrectConfiramtionToken(token);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new Response<AppUser>
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = ERROR_USER_DOES_NOT_EXIST,
                    IsSuccessful = false
                });
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest(new Response<AppUser>
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = ERROR_INVALID_TOKEN,
                    IsSuccessful = false
                });

            return Ok(new Response<AppUser>
            {
                Data = user,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
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
                    ErrorMessage = ERROR_USER_DOES_NOT_EXIST
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
            var user = await _userManager.FindByNameAsync(userRequest.UserName);
            /*if (user != null)
            {
                return BadRequest(new Response<AppUser> {Data = null, ErrorCode = HttpStatusCode.BadRequest.ToString(), 
                ErrorMessage = ERROR_USERNAME_EXISTS, IsSuccessful = false});
            }*/

            var userId = User.GetUserId();
            var newUser = _mapper.Map<AppUser>(userRequest);
            await _userService.EditUser(newUser, userId);

            return Ok(userRequest);
        }


        public string CorrectConfiramtionToken(string token)
        {
            return token.Replace(" ", "+");
        }
    }
}
