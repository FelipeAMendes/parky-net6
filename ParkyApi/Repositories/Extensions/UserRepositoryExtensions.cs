using Microsoft.IdentityModel.Tokens;
using ParkyApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkyApi.Repositories.Extensions;

public static class UserRepositoryExtensions
{
    public static string GetToken(this UserRepository _, User user)
    {
        var secret = Environment.GetEnvironmentVariable("JwtSecret");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var secutiryToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(secutiryToken);

        return token;
    }
}