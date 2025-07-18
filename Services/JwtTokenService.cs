using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cortado.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace cortado.Services;

public class JwtTokenService(IConfiguration config)
{
    public LoginResponse GenerateToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims:
            [
                new Claim(ClaimTypes.Name, username)
            ],
            expires: DateTime.UtcNow.AddMinutes(double.Parse(config["JwtSettings:ExpiresInMinutes"])),
            signingCredentials: creds);

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        };
    }
}