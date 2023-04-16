using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.JwtTokenManager;

public record JwtTokenManagerOptions
{
    public SymmetricSecurityKey Secret { get; }
    public TimeSpan Expire { get; }
    public string Issuer { get; }
    public string Audience { get; }

    public JwtTokenManagerOptions(SymmetricSecurityKey secret, TimeSpan expire, string issuer, string audience)
    {
        Secret = secret;
        Expire = expire;
        Issuer = issuer;
        Audience = audience;
    }
}