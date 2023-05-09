using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtTokenHelper;

public class JwtTokenHelper : IJwtTokenHelper
{
    private readonly JwtTokenHelperOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenHelper(JwtTokenHelperOptions options)
    {
        _options = options;
        _validationParameters = new TokenValidationParameters
        {
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            IssuerSigningKey = options.Secret,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
        };
    }
    
    public string CreateJwtToken(Guid userId)
    {
        SigningCredentials credentials = new(_options.Secret, SecurityAlgorithms.HmacSha256);
        
        Claim[] claims = {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _options.Audience),
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.Expire),
            SigningCredentials = credentials
        };

        return _tokenHandler.CreateEncodedJwt(tokenDescriptor);
    }

    public Guid ValidateAndExtractUserId(string jwtToken)
    {
        Validate(jwtToken);
        return ExtractUserId(jwtToken);
    }

    public Guid ExtractUserId(string jwtToken)
    {
        return new Guid(_tokenHandler.ReadJwtToken(jwtToken).Subject);
    }

    private void Validate(string token)
    {
        _tokenHandler.ValidateToken(token, _validationParameters, out _);
    }
}