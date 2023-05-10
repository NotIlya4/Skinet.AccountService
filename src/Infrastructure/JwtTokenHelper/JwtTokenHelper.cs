using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtTokenHelper;

public class JwtTokenHelper : IJwtTokenHelper, IJwtTokenValidator
{
    private readonly JwtTokenHelperOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly TokenValidationParameters _validationParameters;
    private readonly SymmetricSecurityKey _secret;

    public JwtTokenHelper(JwtTokenHelperOptions options)
    {
        _options = options;
        _secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
        _validationParameters = new TokenValidationParameters
        {
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            IssuerSigningKey = _secret,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
        };
    }
    
    public JwtToken Create(UserId userId)
    {
        SigningCredentials credentials = new(_secret, SecurityAlgorithms.HmacSha256);
        
        Claim[] claims = {
            new(JwtRegisteredClaimNames.Sub, userId.Value.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iss, _options.Issuer),
            new(JwtRegisteredClaimNames.Aud, _options.Audience),
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.Expire),
            SigningCredentials = credentials
        };

        string rawJwtToken = _tokenHandler.CreateEncodedJwt(tokenDescriptor);
        return new JwtToken(rawJwtToken);
    }

    ValidJwtToken IJwtTokenHelper.Validate(string jwtToken)
    {
        _tokenHandler.ValidateToken(jwtToken, _validationParameters, out _);
        return new ValidJwtToken(this, jwtToken);
    }

    ValidJwtToken IJwtTokenHelper.Validate(JwtToken jwtToken)
    {
        return ((IJwtTokenHelper)this).Validate(jwtToken.Raw);
    }
    
    void IJwtTokenValidator.Validate(string jwtToken)
    {
        _tokenHandler.ValidateToken(jwtToken, _validationParameters, out _);
    }
}