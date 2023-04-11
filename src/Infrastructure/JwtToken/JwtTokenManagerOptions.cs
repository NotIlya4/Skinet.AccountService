using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtToken;

public class JwtTokenManagerOptions
{
    public required SymmetricSecurityKey JwtTokenSecret { get; init; }
    public required TimeSpan JwtTokenExpireTime { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}