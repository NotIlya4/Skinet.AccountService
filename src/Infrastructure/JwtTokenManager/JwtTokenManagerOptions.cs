using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtTokenManager;

public class JwtTokenManagerOptions
{
    public required SymmetricSecurityKey Secret { get; init; }
    public required TimeSpan Expire { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}