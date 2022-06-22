using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private const string JWT_SETTINGS_SECTION = "JwtSettings";
        private const string SECURITY_KEY_SECTION = "securityKey";
        private const string VALID_ISSUER_SECTION = "validIssuer";
        private const string VALID_AUDIENCE_SECTION = "validAudience";
        private const string EXPIRY_IN_MINUTES_SECTION = "expiryInMinutes";

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection(JWT_SETTINGS_SECTION);
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings[VALID_ISSUER_SECTION],
                audience: _jwtSettings[VALID_AUDIENCE_SECTION],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings[EXPIRY_IN_MINUTES_SECTION])),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }

        public List<Claim> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            return claims;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection(SECURITY_KEY_SECTION).Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public string GetToken(AppUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
