using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cookify.Api.Abstractions;
using Cookify.Api.Model;
using Microsoft.IdentityModel.Tokens;

namespace Cookify.Api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(User user, IList<string> roles)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("displayName", user.Username ?? user.UserName!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}