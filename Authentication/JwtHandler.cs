using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TenderAPI.Models;

namespace TenderAPI.Authentication
{
    public class JwtHandler
    {
        // Creazione del token
        public static string GenerateJwtToken(User user, string secretKey, string issuer, string audience)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Cosa inserire nel token
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(1), // Scadenza del token
                    signingCredentials: credentials
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
