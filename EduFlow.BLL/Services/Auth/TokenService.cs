using EduFlow.BLL.Interfaces.Auth;
using EduFlow.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduFlow.BLL.Services.Auth;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly IConfiguration _configuration = configuration.GetSection("Jwt");
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname),
            new Claim("PhoneNumber", user.PhoneNumber),
            new Claim("Age", user.Age.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var secutiryKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
        var credentials = new SigningCredentials(secutiryKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDestrictor = new JwtSecurityToken(
            issuer: _configuration["Issuer"],
            audience: _configuration["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Lifetime"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDestrictor);
    }
}
