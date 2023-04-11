using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtToken;

public class JwtTokenManager
{
    private readonly JwtTokenManagerOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenManager(JwtTokenManagerOptions options)
    {
        _options = options;
        _validationParameters = new TokenValidationParameters
        {
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            IssuerSigningKey = options.JwtTokenSecret,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
        };
    }
    
    public string CreateJwtToken(UserId userId)
    {
        SigningCredentials credentials = new(_options.JwtTokenSecret, SecurityAlgorithms.HmacSha256);
        
        Claim[] claims = {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _options.Audience),
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.JwtTokenExpireTime),
            SigningCredentials = credentials
        };

        return _tokenHandler.CreateEncodedJwt(tokenDescriptor);
    }

    public UserId ValidateAndGetUserId(string token)
    {
        _tokenHandler.ValidateToken(token, _validationParameters, out _);

        return new UserId(new JwtSecurityToken(token).Subject);
    }
}