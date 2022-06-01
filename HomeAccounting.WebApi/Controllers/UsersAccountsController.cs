using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs.AuthentificationDTOs;
using HomeAccounting.WebApi.DTOs.RegistrationDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public UsersAccountsController(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
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

            return Ok(userForRegistration);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationRequestDTO userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new UserAuthenticationResponseDTO { ErrorMessage = "Invalid Authentication" });
            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new UserAuthenticationResponseDTO { IsAuthSuccessful = true, Token = token });
        }
    }
}
