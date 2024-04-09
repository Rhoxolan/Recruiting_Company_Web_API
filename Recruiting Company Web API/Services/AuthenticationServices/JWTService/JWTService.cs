using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService
{
    public class JWTService(IConfiguration configuration) : IJWTService
    {
		public string GenerateJWTToken(IdentityUser user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName!)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
