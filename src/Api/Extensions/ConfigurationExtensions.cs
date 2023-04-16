using System.Text;
using Infrastructure.JwtTokenManager;
using Infrastructure.RefreshTokenSystem.Repository;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class ConfigurationExtensions
{
    public static JwtTokenManagerOptions GetJwtTokenManagerOptions(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            config = config.GetSection(key);
        }
        
        SymmetricSecurityKey secret = new(Encoding.UTF8.GetBytes(config.GetRequiredValue("Secret")));
        TimeSpan expire = TimeSpan.FromMinutes(config.GetRequiredValue<float>("ExpireMinutes"));
        string issuer = config.GetRequiredValue("Issuer");
        string audience = config.GetRequiredValue("Audience");

        return new JwtTokenManagerOptions(
            secret: secret,
            expire: expire,
            issuer: issuer,
            audience: audience);
    }

    public static RefreshTokenRepositoryOptions GetRefreshTokenRepositoryOptions(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            config = config.GetSection(key);
        }
        
        TimeSpan expire = TimeSpan.FromHours(config.GetRequiredValue<float>("ExpireHours"));

        return new RefreshTokenRepositoryOptions(expire);
    }
}