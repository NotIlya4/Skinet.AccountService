﻿using System.Text;
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
        TimeSpan expire = TimeSpan.FromMinutes(config.GetRequiredValue<int>("ExpireMinutes"));
        string issuer = config.GetRequiredValue("Issuer");
        string audience = config.GetRequiredValue("Audience");

        return new JwtTokenManagerOptions()
        {
            Secret = secret,
            Expire = expire,
            Issuer = issuer,
            Audience = audience
        };
    }

    public static RefreshTokenRepositoryOptions GetRefreshTokenRepositoryOptions(this IConfiguration config, string? key = null)
    {
        if (key is not null)
        {
            config = config.GetSection(key);
        }
        
        TimeSpan expire = TimeSpan.FromHours(config.GetRequiredValue<int>("ExpireHours"));

        return new RefreshTokenRepositoryOptions()
        {
            Expire = expire
        };
    }
}