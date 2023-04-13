using System.Text;
using Api.Extensions;
using Infrastructure.JwtTokenManager;
using Infrastructure.RefreshTokenSystem.Repository;
using Microsoft.IdentityModel.Tokens;

namespace Api.Properties;

public class ParametersProvider
{
    private readonly IConfiguration _config;

    public ParametersProvider(IConfiguration config)
    {
        _config = config;
    }

    public RefreshTokenRepositoryOptions GetRefreshTokenRepositoryOptions()
    {
        return _config.GetRefreshTokenRepositoryOptions("RefreshTokenRepositoryOptions");
    }

    public JwtTokenManagerOptions GetJwtTokenManagerOptions()
    {
        return _config.GetJwtTokenManagerOptions("JwtTokenManagerOptions");
    }

    public string GetSqlServer()
    {
        return _config.GetSqlServerConnectionString("SqlServer");
    }
    
    public string GetRedis()
    {
        return _config.GetRedisConnectionString("Redis");
    }

    public bool AutoMigrate()
    {
        return _config.GetRequiredValue<bool>("AutoMigrate");
    }

    public bool AutoSeed()
    {
        return _config.GetRequiredValue<bool>("AutoSeed");
    }
}