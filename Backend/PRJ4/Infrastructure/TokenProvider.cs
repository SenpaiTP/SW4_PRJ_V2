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

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, bruger.BrugerId.ToString()),
                new Claim("Navn", bruger.Navn),
                new Claim(JwtRegisteredClaimNames.Email, bruger.Email)
            };

            var ClaimsIdentity=new ClaimsIdentity(claims);
            
            string secretKey = _configuration["Jwt:SecretKey"];
            var secruityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(secruityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, bruger.BrugerId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, bruger.Email)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var handler=new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;

        }
    }

}