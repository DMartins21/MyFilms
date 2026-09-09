using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace myfilms.Services;

public class TokenService : ITokenService
{
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
    { 
        var key = _config.GetSection("JWT").GetValue<string>("SecretKey")
                ?? throw new InvalidOperationException("Invalid secret Key");
        
        var privateKey = Encoding.UTF8.GetBytes(key);

        var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_config.GetSection("JWT")
                    .GetValue<double>("TokenValidInHours")), 
                Audience = _config.GetSection("JWT")
                    .GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT")
                    .GetValue<string>("ValidIssuer"),
                SigningCredentials = signinCredentials
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
    {
        throw new NotImplementedException();
    }
}