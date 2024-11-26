using System;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using PRJ4.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;


namespace PRJ4.Infrastructure
{
    public class TokenProvider
    {
        private readonly IConfiguration _configuration;

        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Create(Bruger bruger)
        {
            // Define the claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, bruger.BrugerId.ToString()), // User ID
                new Claim(JwtRegisteredClaimNames.Email, bruger.Email)              // User Email
            };

            // Get secret key from configuration
            string secretKey = _configuration["Jwt:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            // Create and return the token
            var handler = new JsonWebTokenHandler();
            var token= handler.CreateToken(tokenDescriptor);

            return token;

        }
    }

}