using DOL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DOL.Services;

public interface ITokenService
{
    (string token, DateTime expiresAt) GenAccessToken(User user);
}

public class TokenService : ITokenService
{
    private readonly string _secret;
    private readonly string _issuer;

    public TokenService(IConfiguration config)
    {
        _secret = config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
        _issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
    }

    public (string token, DateTime expiresAt) GenAccessToken(User user)
    {
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(15);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("isMerchant", user.IsMerchant.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
           issuer: _issuer,
           claims: claims,
           expires: expiresAt,
           signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
