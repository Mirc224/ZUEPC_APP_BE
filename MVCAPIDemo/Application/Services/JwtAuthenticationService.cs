using Constants.Infrastructure;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using MVCAPIDemo.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MVCAPIDemo.Application.Services;

public class JwtAuthenticationService
{

    private readonly JwtSettings _jwtSettings;
    public JwtAuthenticationService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }
    public string CreateToken(UserModel userModel)
    {
        List<Claim> claims = new List<Claim>()
        {
			new Claim(Claims.Email, userModel.Email),
            new Claim(Claims.UserId, userModel.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
