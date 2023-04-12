using System.Text;
using Infrastructure.JwtToken;
using Infrastructure.RefreshToken;
using Microsoft.IdentityModel.Tokens;

namespace Api.Properties;

public class ParametersProvider
{
    private readonly IConfiguration _config;

    public ParametersProvider(IConfiguration config)
    {
        _config = config;
    }

    private TimeSpan GetJwtTokenExpireTime()
    {
        return GetTimeSpan("JwtToken:ExpireTime");
    }

    private string GetJwtTokenIssuer()
    {
        return GetRequiredParameter<string>("JwtToken:Issuer");
    }
    
    private string GetJwtTokenAudience()
    {
        return GetRequiredParameter<string>("JwtToken:Audience");
    }

    private TimeSpan GetRefreshTokenExpireTime()
    {
        return GetTimeSpan("RefreshToken:ExpireTime");
    }

    private SymmetricSecurityKey GetJwtTokenSecret()
    {
        string rawSecret = GetRequiredParameter<string>("JwtToken:Secret");
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawSecret));
    }

    public TokenRepositoryOptions GetTokenRepositoryOptions()
    {
        TimeSpan refreshTokenExpire = GetRefreshTokenExpireTime();
        return new TokenRepositoryOptions()
        {
            JwtRefreshTokenExpireTime = refreshTokenExpire
        };
    }

    public JwtTokenManagerOptions GetJwtTokenManagerOptions()
    {
        return new JwtTokenManagerOptions()
        {
            Issuer = GetJwtTokenIssuer(),
            Audience = GetJwtTokenAudience(),
            JwtTokenSecret = GetJwtTokenSecret(),
            JwtTokenExpireTime = GetJwtTokenExpireTime()
        };
    }

    private TimeSpan GetTimeSpan(IConfiguration config, string key)
    {
        int milliseconds = GetRequiredParameter<int>(config, key);
        return TimeSpan.FromMilliseconds(milliseconds);
    }

    private TimeSpan GetTimeSpan(string key)
    {
        return GetTimeSpan(_config, key);
    }

    private T GetRequiredParameter<T>(IConfiguration config, string key)
    {
        T? value = config.GetValue<T>(key);
        if (value is null)
        {
            throw new ParameterNotFoundException(key);
        }
        return value;
    }

    private T GetRequiredParameter<T>(string key)
    {
        return GetRequiredParameter<T>(_config, key);
    }
}