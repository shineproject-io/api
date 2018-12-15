using ListsAPI.Infrastructure.Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ListsAPI.Infrastructure.Authentication
{
    public interface IAuthenticationTokenProvider
    {
        AuthenticationToken Generate(string emailAddress, string userProfileId);
    }

    public class AuthenticationTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IConfigurationValueProvider _configurationValueProvider;

        public AuthenticationTokenProvider(IConfigurationValueProvider configurationValueProvider)
        {
            _configurationValueProvider = configurationValueProvider;
        }

        private string _securityKey
        {
            get
            {
                return _configurationValueProvider.GetValue<string>("SymmetricSecurityKey");
            }
        }

        private DateTime ExpirationDateTime
        {
            get
            {
                return DateTime.UtcNow.AddHours(24);
            }
        }

        public AuthenticationToken Generate(string emailAddress, string userProfileId)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, emailAddress),
                new Claim(ClaimTypes.Name, userProfileId),
                new Claim(JwtRegisteredClaimNames.Email, emailAddress)
            };

            var token = new JwtSecurityToken(issuer: "ListsApi", audience: "ListsWeb", claims: claims, notBefore: DateTime.Now, expires: ExpirationDateTime, signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));

            return new AuthenticationToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationDateTime = ExpirationDateTime
            };
        }
    }
}